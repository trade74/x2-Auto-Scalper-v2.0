using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;


namespace cAlgo.Robots
{

    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class x2AutoScalperV2 : Robot
    {
        #region Params
        //Bollinger Bands
        [Parameter("Source", Group = "Bollinger Bands")]
        public DataSeries BollingBandsSource { get; set; }

        [Parameter("Period", Group = "Bollinger Bands", DefaultValue = 50)]
        public int BollingBandsPeriod { get; set; }

        [Parameter("Std Deviation", Group = "Bollinger Bands", DefaultValue = 2.0)]
        public double BollingBandsSD { get; set; }



        [Parameter("MA Type", Group = "Bollinger Bands", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType BollingBandsMAType { get; set; }

        //MA Cloud Settings
        [Parameter("Source", Group = "EMA")]
        public DataSeries EMASource { get; set; }

        [Parameter("Fast MA Period", Group = "EMA", DefaultValue = 8)]
        public int EMAFastPeriod { get; set; }

        [Parameter("Slow MA Period", Group = "EMA", DefaultValue = 21)]
        public int EMASlowPeriod { get; set; }

        [Parameter("Label ( Magic Name )", Group = "Identity", DefaultValue = "BB Gun")]
        public string MyLabel { get; set; }


        [Parameter("Slippage (pips)", Group = "Strategy", DefaultValue = 2.0, MinValue = 0.5, Step = 0.1)]
        public double SLIPPAGE { get; set; }

        [Parameter("Gap", Group = "Strategy", DefaultValue = 10.0)]
        public double GapInPips { get; set; }

        [Parameter("Starting Size", Group = "Strategy", DefaultValue = 1000)]
        public int StartingSize { get; set; }

        [Parameter("Bar to switch To FastEMA Exit", Group = "Strategy", DefaultValue = 4)]
        public int SwitchFastEMA { get; set; }

        [Parameter("ATR To Avoid", Group = "Strategy", DefaultValue = 0.002)]
        public double ATRToAvoid { get; set; }

        [Parameter("Lot Size Multplier", Group = "Strategy", DefaultValue = 2)]
        public int LotSizeMultiplier { get; set; }


        #endregion

        #region Emuns
        public enum LoopType
        {

            OnBar,
            OnTick

        }
        #endregion



        private ExponentialMovingAverage emaFast;
        private ExponentialMovingAverage emaSlow;
        private BollingerBands BB;
        private AverageTrueRange ATR;

        public int BuyPositions = 0;
        public int SellPositions = 0;
        public Position FirstPosition = null;
        public Position LastPosition = null;
        public Position longPositions = null;
        public Position shortPositions = null;

        TradeType direction;


        protected override void OnStart()
        {
            // Put your initialization logic here,
            BB = Indicators.BollingerBands(BollingBandsSource, BollingBandsPeriod, BollingBandsSD, BollingBandsMAType);
            emaFast = Indicators.ExponentialMovingAverage(EMASource, EMAFastPeriod);
            emaSlow = Indicators.ExponentialMovingAverage(EMASource, EMASlowPeriod);
            ATR = Indicators.AverageTrueRange(2, MovingAverageType.Hull);

            Positions.Opened += _onOpenPositions;
        }

        protected override void OnTick()
        {
            // Put your core logic here

            loop();
        }

        protected override void OnStop()
        {
            // -->Better to eliminate the handler, it shouldn't be used but you never know
            Positions.Opened -= _onOpenPositions;
        }

        private void loop()
        {
            //Trade management of open positons
            if (!zeroOpenLongPositions() && priceGappedDown())
                openMarketOrderBuy("Order" + MyLabel);

            if (!zeroOpenShortPositions() && priceGappedUp())
                openMarketOrderSell("Order" + MyLabel);

            //Check to see if we should close all positions
            if (!zeroOpenLongPositions() && checkForExitLongTrades())
                exitAllLongTrades();

            if (!zeroOpenShortPositions() && checkForExitShortTrades())
                exitAllShortTrades();

            //Check if triggers can start an active trade
            if (filteredTriggerLong() && zeroOpenLongPositions())
                openMarketOrderBuy("Order" + MyLabel);

            if (filteredTriggerShort() && zeroOpenShortPositions())
                openMarketOrderSell("Order" + MyLabel);

            //Monitor, Open, Close Hedged Positions
            //   MonitorHedging();

        }

        private void openMarketOrderBuy(string label, bool hedging = false)
        {
            double stopLossInPips = stopLoss();
            double volumeInUnits = hedging ? lotSize(hedging) : lotSize();
            string mylabel = label.Length > 0 ? label : MyLabel;



            var result = ExecuteMarketRangeOrder(TradeType.Buy, SymbolName, volumeInUnits, SLIPPAGE, Symbol.Ask, mylabel, stopLossInPips, 100);
            if (result.IsSuccessful)
            {
                Print("BUY: Volume: {0} Ask: {1} Slippage {2} StopLoss: {3} SymbolName: {4}", volumeInUnits, Symbol.Bid, SLIPPAGE, stopLossInPips, SymbolName);
            }
            else
            {
                Stop();
            }

        }

        private void openMarketOrderSell(string label, bool hedging = false)
        {
            double stopLossInPips = stopLoss();
            double volumeInUnits = hedging ? lotSize(hedging) : lotSize();
            string mylabel = label.Length > 0 ? label : MyLabel;

            ExecuteMarketRangeOrder(TradeType.Sell, SymbolName, volumeInUnits, SLIPPAGE, Symbol.Bid, mylabel, stopLossInPips, 100);

        }

        private void exitAllLongTrades()
        {
            foreach (var position in Positions)
            {
                if (position.TradeType == TradeType.Buy)
                    Trade.Close(position);
            }
        }

        private void exitAllShortTrades()
        {
            foreach (var position in Positions)
            {
                if (position.TradeType == TradeType.Sell)
                    Trade.Close(position);
            }
        }

        private bool checkForExitLongTrades()
        {
            if (EMASlowFilterLongExit())
                return true;

            if (EMAFastFilterLongExit())
                return true;

                        /*   if (ATRFilterExit())
                return true;*/


return false;
        }

        private void exitHedgedTrades()
        {
            foreach (var position in Positions)
            {
                if (position.Label == "Hedge" + MyLabel)
                    Trade.Close(position);
            }
        }

        private bool checkForExitShortTrades()
        {
            if (EMASlowFilterShortExit())
                return true;

            if (EMAFastFilterShortExit())
                return true;

                        /*if (ATRFilterExit())
                return true;
*/
return false;
        }

        private bool zeroOpenLongPositions()
        {
            // Print("Positions.Count: ", Positions.Count);

            if (Positions.FindAll("Order" + MyLabel, SymbolName, TradeType.Buy).Length == 0)
                return true;

            return false;


        }

        private bool zeroOpenShortPositions()
        {
            // Print("Positions.Count: ", Positions.Count);

            if (Positions.FindAll("Order" + MyLabel, SymbolName, TradeType.Sell).Length == 0)
                return true;

            return false;


        }


        private bool priceGappedDown()
        {
            if (LastPosition == null)
                return false;

            double lastEntryPrice = LastPosition.EntryPrice;
            double gapPrice = lastEntryPrice - PipsToDigits(Symbol, GapInPips * 1.2);

            //  Print("GapPrice {0}, LastPosition.EntryPrice: {1}, Symbol.Ask: {2}", gapPrice, lastEntryPrice, Symbol.Ask);

            if (Symbol.Ask < gapPrice)
                return true;

            return false;
        }

        private bool priceGappedUp()
        {
            if (LastPosition == null)
                return false;

            double lastEntryPrice = LastPosition.EntryPrice;
            double gapPrice = lastEntryPrice + PipsToDigits(Symbol, GapInPips * 1.2);

            // Print("GapPrice {0}, LastPosition.EntryPrice: {1}, Symbol.Ask: {2}", gapPrice, lastEntryPrice, Symbol.Ask);

            if (Symbol.Ask > gapPrice)
                return true;

            return false;
        }



        #region TRIGGERS


        private bool filteredTriggerLong()
        {
            if (BollingBandFilterLong())
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        private bool filteredTriggerShort()
        {
            if (BollingBandFilterShort())
                return true;

            return false;
        }

        private bool timeTrigger()
        {
            return true;
        }

        private bool accountTrigger()
        {
            return true;
        }

        #endregion



        #region FILTERS
        ///Build boolean filters for indicators that need to pass true 

        #region Bollinger Band Filter
        private bool BollingBandFilterLong()
        {
            if (Symbol.Ask < BB.Bottom.LastValue)
            {
                // Print("BBFilter Triggered.  BB.Bottom.LastValue: {0}, Symbol.Ask {1}", BB.Bottom.LastValue, Symbol.Ask);
                return true;
            }
            else
            {

                return false;
            }

        }

        private bool BollingBandFilterShort()
        {
            if (Symbol.Bid > BB.Top.LastValue)
            {
                //  Print("BBFilter Triggered.  BB.Top.LastValue: {0}, Symbol.Bid {1}", BB.Top.LastValue, Symbol.Bid);
                return true;
            }
            else
            {

                return false;
            }
        }
        #endregion

        #region TD seq Filter
        private bool TDFilterLong()
        {
            return true;
        }

        private bool TDFilterShort()
        {
            return true;
        }

        private bool EMASlowFilterLongExit()
        {
            if (Symbol.Bid > emaSlow.Result.LastValue && Positions.Count < SwitchFastEMA)
                return true;

            return false;

        }

        private bool EMASlowFilterShortExit()
        {
            if (Symbol.Ask < emaSlow.Result.LastValue && Positions.Count < SwitchFastEMA)
                return true;

            return false;

        }

        private bool ATRFilterExit()
        {
            if (ATR.Result.LastValue > ATRToAvoid)
            {
                Print("ATR trigger true");
                return true;
            }

            return false;

        }

        private bool EMAFastFilterLongExit()
        {
            if (Symbol.Bid > emaFast.Result.LastValue && Positions.Count >= SwitchFastEMA)
                return true;

            return false;

        }

        private bool EMAFastFilterShortExit()
        {
            if (Symbol.Ask < emaFast.Result.LastValue && Positions.Count >= SwitchFastEMA)
                return true;

            return false;

        }

        #endregion

        #endregion

        #region MONEY MANAGEMENT

        private double lotSize(bool hedging = false)
        {
            double vol = Positions.Count == 0 ? StartingSize : StartingSize * Positions.Count * LotSizeMultiplier;
            double size;

            if (hedging)
            {
                var buyTotalVolume = Positions.Where(x => x.TradeType == TradeType.Buy && x.Label == "Order" + MyLabel).Sum(x => x.VolumeInUnits);
                var sellTotalVolume = Positions.Where(x => x.TradeType == TradeType.Sell && x.Label == "Order" + MyLabel).Sum(x => x.VolumeInUnits);

                vol = buyTotalVolume > 0 ? buyTotalVolume : sellTotalVolume;
            }



            return Symbol.NormalizeVolumeInUnits(vol, RoundingMode.Down);
        }

        private int stopLoss()
        {
            return 200;
        }

        #endregion

        #region TRADE MANAGEMENT

        #region TRADE MANAGEMENT - Martingale

        #endregion

        #region TRADE MANAGEMENT - Hedging
        private void MonitorHedging()
        {
            //Open a hedge if our profit is below param eg. less than -$500
            double profit = 0;


            foreach (Position p in Positions.FindAll("Order" + MyLabel))
            {
                profit += p.GrossProfit;
                direction = p.TradeType;
            }


            if (profit < -5 && Positions.FindAll("Hedge" + MyLabel).Length == 0)
            {
                Print("Profit {0}", profit);
                //Open a counter Hedge trade
                if (direction == TradeType.Sell)
                    openMarketOrderBuy("Hedge" + MyLabel);

                if (direction == TradeType.Buy)
                    openMarketOrderSell("Hedge" + MyLabel);


            }





            //If any Open Hedged positions check profit and close if the Hedging profit goes negative $
            if (checkForExitHedgeTrades())
            {
                exitHedgedTrades();
            }


        }

        private bool checkForExitHedgeTrades()
        {
            double profit = 0;


            foreach (Position p in Positions.FindAll("Hedge" + MyLabel))
            {
                profit += p.GrossProfit;
                direction = p.TradeType;
            }

            if (profit < -3)
                return true;

            return false;
        }




        #endregion

        #region TRADE MANAGEMENT - Break Even

        #endregion

        #region TRADE MANAGEMENT - Exit Trade

        private bool exitLong()
        {
            //Exit a long trade if the price returns and crosses the Moving Average. 

            return true;
        }

        #endregion



        #endregion

        #region EVENTS
        private void _onOpenPositions(PositionOpenedEventArgs eventArgs)
        {

            if (eventArgs.Position.SymbolName == Symbol.Name && eventArgs.Position.Label == "Order" + MyLabel)
            {

                LastPosition = eventArgs.Position;
                Print("Position.EntryPrice {0}", LastPosition.EntryPrice);


            }

        }
        #endregion

        /// <summary>
        /// Converts the number of pips current from Digits to Double
        /// </summary>
        /// <param name="Pips">The number of pips in the Digits format</param>
        /// <returns></returns>
        public double DigitsToPips(Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips / thisSymbol.PipSize, 2);

        }

        /// <summary>
        /// Converts the number of pips current from Double to Digits
        /// </summary>
        /// <param name="Pips">The number of pips in the Double format (2)</param>
        /// <returns></returns>
        public double PipsToDigits(Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips * thisSymbol.PipSize, thisSymbol.Digits);


        }
    }
}
