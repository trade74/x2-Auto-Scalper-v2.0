/ *CTRADER GURU
   Homepage: https://ctrader.guru/
Telegram: https://t.me/ctraderguru
Twitter: https://twitter.com/cTraderGURU/
Facebook: https://www.facebook.com/ctrader.guru/
YouTube: https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
GitHub: https://github.com/ctrader-guru
* /

using System;
using System. Collections. Generic;
using cAlgo. API;
using cAlgo. API. Internals;

namespace cAlgo
{
    /// <summary>
    /// Extensions that make code smoother with methods not provided by the cAlgo library
    /// </summary>
    public static class Extensions
    {

        #region Enum

        /// <summary>
        /// Enumerator to expose the color name in the options
        /// </summary>
        public enum ColorNameEnum
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarines,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            Medium Turquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            Navajo White,
            Navy,
            OldLace,
            Olives,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            Peachpuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        /// <summary>
        /// Enumerator to expose a drop-down menu choice in parameters
        /// </summary>
        public enum CapitalTo
        {

            Balance,
            Equity

        }

        /// <summary>
        /// Enumerate the possibility of choosing the direction of profit
        /// </summary>
        public enum ProfitDirection
        {

            All,
            Positive,
            Negative

        }

        public enum OpenTradeType
        {

            All,
            Buy,
            Sell

        }

        #endregion

        #region class

        /// <summary>
        /// Class to monitor the positions of a specific strategy
        /// </summary>
        public class Monitor
        {

            private Positions _allPositions = null;

            /// <summary>
            /// Standard for collecting information in the Monitor
            /// </summary>
            public class Information
            {

                public double TotalNetProfit = 0;
                public double MinVolumeInUnits = 0;
                public double MaxVolumeInUnits = 0;
                public double MidVolumeInUnits = 0;
                public int BuyPositions = 0;
                public int SellPositions = 0;
                public Position FirstPosition = null;
                public Position LastPosition = null;
                public double HighestHighAfterFirstOpen = 0;
                public double LowestLowAfterFirstOpen = 0;
                public double TotalLotsBuy = 0;
                public double TotalLotsSell = 0;
                public bool IAmInHedging = false;

            }

            /// <summary>
            /// Standard for the interpretation of the time in double
            /// </summary>
            public class PauseTimes
            {

                public double Over = 0;
                public double Under = 0;

            }

            /// <summary>
            /// Standard for break even management
            /// </summary>
            public class BreakEvenData
            {

                // -> In case of multiple operations it would be good to avoid the management of all
                public bool OnlyFirst = false;
                public ProfitDirection ProfitDirection = ProfitDirection.All;
                public double Activation = 0;
                public int LimitBar = 0;
                public double Distance = 0;

            }

            /// <summary>
            /// Trailing Management Standards
            /// </summary>
            public class TrailingData
            {

                // -> In case of multiple operations it would be good to avoid the management of all
                public bool OnlyFirst = false;
                public bool ProActive = false;
                public double Activation = 0;
                public double Distance = 0;

            }

            /// <summary>
            /// Stores the opening status of an operation in the current Bar
            /// </summary>
            public bool OpenedInThisBar = false;

            /// <summary>
            /// Stores the open state of an operation with the current trigger
            /// </summary>
            public bool OpenedInThisTrigger = false;

            /// <summary>
            /// Unique value that identifies the strategy
            /// </summary>
            public readonly string Label;

            /// <summary>
            /// The Symbol to be monitored in relation to the Label
            /// </summary>
            public readonly Symbol Symbol;

            /// <summary>
            /// The Bars with which the strategy moves and elaborates its conditions
            /// </summary>
            public readonly Bars Bars;

            /// <summary>
            /// The time reference of the pause
            /// </summary>
            public readonly PauseTimes Pause;

            /// <summary>
            /// The information collected after the .Update () call
            /// </summary>
            public Information Info { get; private set; }

            /// <summary>
            /// Positions filtered by symbol and label
            /// </summary>
            public Position[] Positions { get; private set; }

            /// <summary>
            /// Monitor for the collection of information regarding the current strategy
            /// </summary>
            public Monitor(string NewLabel, Symbol NewSymbol, Bars NewBars, Positions AllPositions, PauseTimes NewPause)
            {

                Label = NewLabel;
                Symbol = NewSymbol;
                Bars = NewBars;
                Pause = NewPause;

                Info = new Information();

                _allPositions = AllPositions;

                // -> We make information available immediately
                Update(false, null, null, 0);

            }

            /// <summary>
            /// Filters and makes information available for the monitored strategy. Eventually Closes and manages operations
            /// </summary>
            public Information Update(bool closeall, BreakEvenData breakevendata, TrailingData trailingdata, double SafeLoss, TradeType? filtertype = null)
            {

                // -> I collect the information I need to get the pulse of the strategy
                Positions = _allPositions.FindAll(Label, Symbol.Name);

                // -> I have to drag and drop the old data before updating it as ceilings
                double highestHighAfterFirstOpen = (Positions.Length > 0) ? Info.HighestHighAfterFirstOpen : 0;
                double lowestLowAfterFirstOpen = (Positions.Length > 0) ? Info.LowestLowAfterFirstOpen : 0;

                // -> Reset information
                Info = new Information
                {

                    // -> Initialize with old data
                    HighestHighAfterFirstOpen = highestHighAfterFirstOpen,
                    LowestLowAfterFirstOpen = lowestLowAfterFirstOpen

                };

                double tmpVolume = 0;

                foreach (Position position in Positions)
                {

                    // -> For proactive trailing and other features I need to know the current state
                    if (Info.HighestHighAfterFirstOpen == 0 || Symbol.Ask > Info.HighestHighAfterFirstOpen)
                        Info.HighestHighAfterFirstOpen = Symbol.Ask;
                    if (Info.LowestLowAfterFirstOpen == 0 || Symbol.Bid < Info.LowestLowAfterFirstOpen)
                        Info.LowestLowAfterFirstOpen = Symbol.Bid;

                    // -> First I have to check whether to close the position
                    if (closeall && (filtertype == null || position.TradeType == filtertype))
                    {

                        position.Close();
                        continue;

                    }

                    if (SafeLoss > 0 && position.StopLoss == null)
                    {

                        TradeResult result = position.ModifyStopLossPips(SafeLoss);

                        // -> Too much voaltility could lead to incorrect proportions and values, however let's not go further
                        if (result.Error == ErrorCode.InvalidRequest || result.Error == ErrorCode.InvalidStopLossTakeProfit)
                        {

                            position.Close();

                        }

                        continue;

                    }

                    // -> Then it's up to break even
                    if (breakevendata! = null && (!breakevendata.OnlyFirst || Positions.Length == 1))
                        _checkBreakEven(position, breakevendata);

                    // -> Then it's up to trailing
                    if (trailingdata! = null && (!trailingdata.OnlyFirst || Positions.Length == 1))
                        _checkTrailing(position, trailingdata);

                    Info.TotalNetProfit + = position.NetProfit;
                    tmpVolume + = position.VolumeInUnits;

                    switch (position.TradeType)
                    {
                        TradeType.Buy houses:

                            Info.BuyPositions++;
                    Info.TotalLotsBuy + = position.Quantity;
                    break;

                    TradeType.Sell case:

                            Info.SellPositions++;
                    Info.TotalLotsSell + = position.Quantity;
                    break;

                }

                if (Info.FirstPosition == null || position.EntryTime < Info.FirstPosition.EntryTime)
                    Info.FirstPosition = position;

                if (Info.LastPosition == null || position.EntryTime > Info.LastPosition.EntryTime)
                    Info.LastPosition = position;

                if (Info.MinVolumeInUnits == 0 || position.VolumeInUnits < Info.MinVolumeInUnits)
                    Info.MinVolumeInUnits = position.VolumeInUnits;

                if (Info.MaxVolumeInUnits == 0 || position.VolumeInUnits > Info.MaxVolumeInUnits)
                    Info.MaxVolumeInUnits = position.VolumeInUnits;

            }

            // -> Returns an Exception Overflow of an arithmetic operation, to be explored
            // Info.MidVolumeInUnits = Symbol.NormalizeVolumeInUnits (tmpVolume / Positions.Length, RoundingMode.ToNearest);
            Info.MidVolumeInUnits = Math.Round(tmpVolume / Positions.Length, 0);
                Info.IAmInHedging = (Positions.Length> 0 && Info.TotalLotsBuy == Info.TotalLotsSell);

                return Info;

            }

        /// <summary>
        /// Close all monitor positions
        /// </summary>
        public void CloseAllPositions(TradeType? filtertype = null)
        {

            Update(true, null, null, 0, filtertype);

        }

        /// <summary>
        /// It establishes if you are in GAP passing a certain distance to measure
        /// </summary>
        public bool InGAP(double distance)
        {

            return Symbol.DigitsToPips(Bars.LastGAP()) > = distance;

        }

        /// <summary>
        /// Check the time slot to determine if it falls within the break time, use double data
        /// because the ctrader does not allow to expose time data, to be updated as soon as the ctrader allows it
        /// </summary>
        /// <returns> Confirm the presence of a time slot in pause </returns>
        public bool InPause(DateTime timeserver)
        {

            // - >> Since we use double data to expose the parameters we have to use mechanisms to translate the time
            string nowHour = (timeserver.Hour < 10) ? string.Format("0 {0}", timeserver.Hour) : string.Format("{0}", timeserver.Hour);
            string nowMinute = (timeserver.Minute < 10) ? string.Format("0 {0}", timeserver.Minute) : string.Format("{0}", timeserver.Minute);

            // -> I establish the control moment in double format
            double now = Convert.ToDouble(string.Format("{0}, {1}", nowHour, nowMinute));

            // -> Elementary comparison to make the logic understandable
            if (Pause.Over < Pause.Under && now > = Pause.Over && now <= Pause.Under)
            {

                return true;

            }
            else if (Pause.Over > Pause.Under && ((now > = Pause.Over && now <= 23.59) || now <= Pause.Under))
            {

                return true;

            }

            return false;

        }

        /// <summary>
        /// Check and make the change in break-even if conditions allow it
        /// </summary>
        private void _checkBreakEven(Position position, BreakEvenData breakevendata)
        {

            if (breakevendata == null || breakevendata.Activation == 0)
                return;

            double activation = Symbol.PipsToDigits(breakevendata.Activation);

            int currentMinutes = Bars.TimeFrame.ToMinutes();
            DateTime limitTime = position.EntryTime.AddMinutes(currentMinutes * breakevendata.LimitBar);
            bool limitActivation = (breakevendata.LimitBar > 0 && Bars.Last(0).OpenTime > = limitTime);

            double distance = Symbol.PipsToDigits(breakevendata.Distance);

            switch (position.TradeType)
            {

                    TradeType.Buy houses:

                        double breakevenpointbuy = Math.Round(position.EntryPrice + distance, Symbol.Digits);

            if (position.StopLoss == breakevenpointbuy || position.TakeProfit == breakevenpointbuy)
                break;

            if ((Symbol.Bid > breakevenpointbuy) && (limitActivation || (breakevendata.ProfitDirection! = ProfitDirection.Negative && (Symbol.Bid > = (position.EntryPrice + activation)))) && (position.StopLoss == null || position.StopLoss < breakevenpointbuy))
            {

                position.ModifyStopLossPrice(breakevenpointbuy);

            }
            else if ((Symbol.Ask < breakevenpointbuy) && (limitActivation || (breakevendata.ProfitDirection! = ProfitDirection.Positive && (Symbol.Bid <= (position.EntryPrice - activation))))) && (position.TakeProfit == null | | position.TakeProfit > breakevenpointbuy))
                        {

                position.ModifyTakeProfitPrice(breakevenpointbuy);

            }

            break;

            TradeType.Sell case:

                        double breakevenpointsell = Math.Round(position.EntryPrice - distance, Symbol.Digits);

            if (position.StopLoss == breakevenpointsell || position.TakeProfit == breakevenpointsell)
                break;

            if ((Symbol.Bid < breakevenpointsell) && (limitActivation || (breakevendata.ProfitDirection! = ProfitDirection.Negative && (Symbol.Ask <= (position.EntryPrice - activation))))) && (position.StopLoss == null || position.StopLoss > breakevenpointsell))
                        {

                position.ModifyStopLossPrice(breakevenpointsell);

            }
                        else if ((Symbol.Ask > breakevenpointsell) && (limitActivation || (breakevendata.ProfitDirection! = ProfitDirection.Positive && (Symbol.Ask > = (position.EntryPrice + activation)))) && (position.TakeProfit == null | | position.TakeProfit < breakevenpointsell))
            {

                position.ModifyTakeProfitPrice(breakevenpointsell);

            }

            break;

        }

    }


    /// <summary>
    /// Check and perform the trailing change if conditions allow it
    /// </summary>
    private void _checkTrailing(Position position, TrailingData trailingdata)
    {

        if (trailingdata == null || trailingdata.Activation == 0 trailingdata ||.Distance == 0)
                    return;

        double trailing = 0;
        double distance = Symbol.PipsToDigits(trailingdata.Distance);
        double activation = Symbol.PipsToDigits(trailingdata.Activation);

        switch (position.TradeType Property)
                {

            TradeType case. Buy:

            trailing = Math.Round(Symbol.Bid - distance, Symbol.Digits);

            if (position.StopLoss == trailing || position.TakeProfit == trailing)
                break;

            if ((Symbol.Bid > = (position.EntryPrice + activation)) & &(position.StopLoss == null || position.StopLoss < trailing))
            {

                position.ModifyStopLossPrice(trailing);

            }
            else if (trailingdata.ProActive & Info.HighestHighAfterFirstOpen > 0 && position.StopLoss! = null && position.StopLoss > 0)
            {

                // -> I need to determine if activation has started
                double activationprice = position.EntryPrice + activation;
                double firsttrailing = Math.Round(activationprice - distance, Symbol.Digits);

                // -> Started trailing? Am I in relegation?
                if (position.StopLoss > = firsttrailing)
                {

                    double limitpriceup = Info.HighestHighAfterFirstOpen;
                    double limitpricedw = Math.Round(Info.HighestHighAfterFirstOpen - distance, Symbol.Digits);

                    double k = Math.Round(limitpriceup - Symbol.Ask, Symbol.Digits);

                    double newtrailing = Math.Round(limitpricedw + k, Symbol.Digits);

                    if (position.StopLoss == newtrailing || position.TakeProfit == newtrailing)
                        break;

                    if (position.StopLoss < newtrailing)
                        position.ModifyStopLossPrice(newtrailing);

                }

            }

            break;

            TradeType case. Sell:

            trailing = Math.Round(Symbol.Ask + Symbol.PipsToDigits(trailingdata.Distance), Symbol.Digits);

            if (position.StopLoss == trailing || position.TakeProfit == trailing)
                break;

            if ((Symbol.Ask <= (position.EntryPrice - Symbol.PipsToDigits(trailingdata.Activation))) & (position.StopLoss == null || position.StopLoss > trailing))
            {

                position.ModifyStopLossPrice(trailing);

            }
            else if (trailingdata.ProActive & Info.LowestLowAfterFirstOpen > 0 && position.StopLoss! = null && position.StopLoss > 0)
            {

                // -> I need to determine if activation has started
                double activationprice = position.EntryPrice - activation;
                double firsttrailing = Math.Round(activationprice + distance, Symbol.Digits);

                // -> Started trailing? Am I in relegation?
                if (position.StopLoss <= firsttrailing)
                {

                    double limitpriceup = Math.Round(Info.LowestLowAfterFirstOpen + distance, Symbol.Digits);
                    double limitpricedw = Info.LowestLowAfterFirstOpen;

                    double k = Math.Round(Symbol.Bid - limitpricedw, Symbol.Digits);

                    double newtrailing = Math.Round(limitpriceup - k, Symbol.Digits);

                    if (position.StopLoss == newtrailing || position.TakeProfit == newtrailing)
                        break;

                    if (position.StopLoss > newtrailing)
                        position.ModifyStopLossPrice(newtrailing);

                }

            }

            break;

        }

    }

}

/// <summary>
/// Class to manage size sizing
/// </summary>
public class MonenyManagement
{

    private readonly double _minSize = 0.01;
    private double _percentage = 0;
    private double _fixedSize = 0;
    private double _pipToCalc = 30;

    // -> References to external objects useful for calculation
    private IAccount _account = null;
    public readonly Symbol Symbol;

    /// <summary>
    /// The capital to be used for the calculation
    /// </summary>
    public CapitalTo CapitalType = CapitalTo.Balance;

    /// <summary>
    /// The percentage of risk you want to invest
    /// </summary>
    public double Percentage
    {

        get { return _percentage; }


        set { _percentage = (value > 0 & value <= 100) ? value : 0; }
    }

    /// <summary>
    /// The fixed size to be used, bypasses all calculation parameters
    /// </summary>
    public double FixedSize
    {

        get { return _fixedSize; }



        set { _fixedSize = (value > = _minSize) ? value : 0; }
    }


    /// <summary>
    /// The maximum distance from the input with which to calculate the size
    /// </summary>
    public double PipToCalc
    {

        get { return _pipToCalc; }

        set { _pipToCalc = (value > 0) ? value : 100; }
    }


    /// <summary>
    /// The actual capital on which to calculate the risk
    /// </summary>
    public double Capital
    {

        get
        {

            switches(CapitalType)
                    {

                CapitalTo houses. Equity:

                return _account.Equity;
                default:


                            return _account.Balance;

            }

        }
    }



    // -> Constructor
    public MonenyManagement(IAccount NewAccount, CapitalTo NewCapitalTo, double NewPercentage, double NewFixedSize, double NewPipToCalc, Symbol NewSymbol)
    {

        _account = NewAccount;

        Symbol = NewSymbol;

        CapitalType = NewCapitalTo;
        Percentage = NewPercentage;
        FixedSize = NewFixedSize;
        PipToCalc = NewPipToCalc;

    }

    /// <summary>
    /// Returns the number of lots in the format 0.01
    /// </summary>
    public double GetLotSize()
    {

        // -> I decided to use a fixed size
        if (FixedSize > 0)
            return FixedSize;

        // -> The percentage of risk in money
        double moneyrisk = Capital / 100 * Percentage;

        // -> I translate the stoploss or its reference into double
        double sl_double = PipToCalc * Symbol.PipSize;

        // -> In format 0.01 = double lots microlot = Math.Round (Symbol.VolumeInUnitsToQuantity (moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);
        // -> In volume format 1K = 1000 Math.Round ((moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);
        double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);

        if (lots < _minSize)
            return _minSize;

        return lots;

    }

}

#endregion

#region Helper

/// <summary>
/// Returns the matching color starting from the name
/// </summary>
/// <returns> The corresponding color </returns>
public static API.Color ColorFromEnum(ColorNameEnum colorName)
{

    return API.Color.FromName(colorName.ToString("G"));

}

#endregion

#region Bars

/// <summary>
/// You get the index of the candle starting from its opening hours
/// </summary>
/// <param name = "MyTime"> The date and time the candle was opened </param>
/// <returns> </returns>
public static int GetIndexByDate(this Bars thisBars, DateTime thisTime)
{

    for (int i = thisBars.ClosePrices.Count - 1; i > = 0; i--)
    {

        if (thisTime == thisBars.OpenTimes[i])
            return i;

    }

    return -1;

}

public static double LastGAP(this Bars thisBars)
{

    double K = 0;

    if (thisBars.ClosePrices.Last(1) > thisBars.OpenPrices.LastValue)
    {

        K = Math.Round(thisBars.ClosePrices.Last(1) - thisBars.OpenPrices.LastValue, 5);

    }
    else if (thisBars.ClosePrices.Last(1) < thisBars.OpenPrices.LastValue)
    {

        K = Math.Round(thisBars.OpenPrices.LastValue - thisBars.ClosePrices.Last(1), 5);

    }

    return K;

}

#endregion

#region Bar

/// <summary>
/// Measure the size of a candle, taking into account its direction
/// </summary>
/// <returns> The body of the candle, value equal to or greater than zero </returns>
public static double Body(this Bar thisBar)
{

    return thisBar.IsBullish() ? thisBar.Close - thisBar.Open : thisBar.Open - thisBar.Close;


}

/// <summary>
/// Check the bullish direction of a candle
/// </summary>
/// <returns> True if the candle is bullish </returns>
public static bool IsBullish(this Bar thisBar)
{

    return thisBar.Close > thisBar.Open;

}

/// <summary>
/// Check the bearish direction of a candle
/// </summary>
/// <returns> True if the candle is bearish </returns>
public static bool IsBearish(this Bar thisBar)
{

    return thisBar.Close < thisBar.Open;

}

/// <summary>
/// Check if a candle has an open equal to the close
/// </summary>
/// <returns> True if the candle is a doji with the same Open and Close </returns>
public static bool IsDoji(this Bar thisBar)
{

    return thisBar.Close == thisBar.Open;

}

#endregion

#region Symbol

/// <summary>
/// Convert the current number of pips from digits to double
/// </summary>
/// <param name = "Pips"> The number of pips in Digits format </param>
/// <returns> </returns>
public static double DigitsToPips(this Symbol thisSymbol, double Pips)
{

    return Math.Round(Pips / thisSymbol.PipSize, 2);

}

/// <summary>
/// Convert the current number of pips from double to digits
/// </summary>
/// <param name = "Pips"> The number of pips in the format Double (2) </param>
/// <returns> </returns>
public static double PipsToDigits(this Symbol thisSymbol, double Pips)
{

    return Math.Round(Pips * thisSymbol.PipSize, thisSymbol.Digits);

}

public static double RealSpread(this Symbol thisSymbol)
{

    return Math.Round(thisSymbol.Spread / thisSymbol.PipSize, 2);

}

#endregion

#region TimeFrame

/// <summary>
/// Returns the current timeframe in minutes
/// </summary>
public static int ToMinutes(this TimeFrame thisTimeFrame)
{

    if (thisTimeFrame == TimeFrame.Daily)
        return 60 * 24;
    if (thisTimeFrame == TimeFrame.Day2)
        return 60 * 24 * 2;
    if (thisTimeFrame == TimeFrame.Day3)
        return 60 * 24 * 3;
    if (thisTimeFrame == TimeFrame.Hour)
        return 60;
    if (thisTimeFrame == TimeFrame.Hour12)
        return 60 * 12;
    if (thisTimeFrame == TimeFrame.Hour2)
        return 60 * 2;
    if (thisTimeFrame == TimeFrame.Hour3)
        return 60 * 3;
    if (thisTimeFrame == TimeFrame.Hour4)
        return 60 * 4;
    if (thisTimeFrame == TimeFrame.Hour6)
        return 60 * 6;
    if (thisTimeFrame == TimeFrame.Hour8)
        return 60 * 8;
    if (thisTimeFrame == TimeFrame.Minute)
        return 1;
    if (thisTimeFrame == TimeFrame.Minute10)
        return 10;
    if (thisTimeFrame == TimeFrame.Minute15)
        return 15;
    if (thisTimeFrame == TimeFrame.Minute2)
        return 2;
    if (thisTimeFrame == TimeFrame.Minute20)
        return 20;
    if (thisTimeFrame == TimeFrame.Minute3)
        return 3;
    if (thisTimeFrame == TimeFrame.Minute30)
        return 30;
    if (thisTimeFrame == TimeFrame.Minute4)
        return 4;
    if (thisTimeFrame == TimeFrame.Minute45)
        return 45;
    if (thisTimeFrame == TimeFrame.Minute5)
        return 5;
    if (thisTimeFrame == TimeFrame.Minute6)
        return 6;
    if (thisTimeFrame == TimeFrame.Minute7)
        return 7;
    if (thisTimeFrame == TimeFrame.Minute8)
        return 8;
    if (thisTimeFrame == TimeFrame.Minute9)
        return 9;
    if (thisTimeFrame == TimeFrame.Monthly)
        return 60 * 24 * 30;
    if (thisTimeFrame == TimeFrame.Weekly)
        return 60 * 24 * 7;

    return 0;

}

        #endregion

    }

}

namespace cAlgo.Robots
{

    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CBOTBASE : Robot
    {

        #region Enums

        public enum MyTradeType
        {

            Disabled,
            Buy,
            Sell

        }

        public enum ProtectionType
        {

            Disabled,
            OnlyFirst,
            Annex

        }

        public enum LoopType
        {

            OnBar,
            OnTick

        }

        public enum StopMode
        {

            Standard,
            Car

        }

        public enum DrawDownMode
        {
            Disabled,
            Close,
            Hedging

        }

        public enum AccCapital
        {
            Balance,
            FreeMargin,
            Equity

        }

        #endregion

        #region Identity

        /// <summary>
        /// Product name, identifier, to be changed with the name of your creation
        /// </summary>
        public const string NAME = "cBot Base";

        /// <summary>
        /// The version of the product, progressive, useful for checking for updates if it is made available on the ctrader.guru website
        /// </summary>
        public const string VERSION = "1.4.5";

        #endregion

        #region Params

        /// <summary>
        /// Product references
        /// </summary>
        [Parameter(NAME + "" + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/cbot-base/")]
        public string ProductInfo { get; set; }

        /// <summary>
        /// Label that identifies an operation
        /// </summary>
        [Parameter("Label (Magic Name)", Group = "Identity", DefaultValue = NAME)]
        public string MyLabel { get; set; }

        /// <summary>
        /// Information on the default preset
        /// </summary>
        [Parameter("Preset information", Group = "Identity", DefaultValue = "Standard preset without any strategy")]
        public string PresetInfo { get; set; }

        [Parameter("Max Cross Coworking (zero disabled)", Group = "Strategy", DefaultValue = 0, MinValue = 0)]
        public int MaxCross { get; set; }

        [Parameter("Loop", Group = "Strategy", DefaultValue = LoopType.OnBar)]
        public LoopType MyLoopType { get; set; }

        [Parameter("Open Trade Type", Group = "Strategy", DefaultValue = Extensions.OpenTradeType.All)]
        public Extensions.OpenTradeType MyOpenTradeType { get; set; }

        [Parameter("Stop", Group = "Strategy", DefaultValue = StopMode.Auto)]
        public StopMode MyStopType { get; set; }

        /// <summary>
        /// The minimum number of pips to consider when removing the broker
        /// </summary>
        [Parameter("Safe StopLoss", Group = "Strategy", DefaultValue = 10, MinValue = 0, Step = 0.1)]
        public double StopLevel { get; set; }

        [Parameter("Boring Close (bars, zero disabled)", Group = "Strategy", DefaultValue = 0, MinValue = 0)]
        public int Boring { get; set; }

        /// <summary>
        /// Activation for the break even moniotropping for one or all trades
        /// </summary>
        [Parameter("Break Even", Group = "Strategy", DefaultValue = ProtectionType.OnlyFirst)]
        public ProtectionType BreakEvenProtectionType { get; set; }

        /// <summary>
        /// The activation for the moniotage of the Trailing for one or for all the trades
        /// </summary>
        [Parameter("Trailing", Group = "Strategy", DefaultValue = ProtectionType.OnlyFirst)]
        public ProtectionType TrailingProtectionType { get; set; }

        [Parameter("Drawdown", Group = "Strategy", DefaultValue = DrawDownMode.Disabled)]
        public DrawDownMode ddMode { get; set; }

        /// <summary>
        /// When this netprofit is reached, it closes everything
        /// </summary>
        [Parameter("Money Target (%, zero disabled)", Group = "Strategy", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double MoneyTargetPercentage { get; set; }

        /// <summary>
        /// I constrain the money target only for a certain number of trades onwards
        /// </summary>
        [Parameter("Money Target Minimum Trades", Group = "Strategy", DefaultValue = 1, MinValue = 1, Step = 1)]
        public int MoneyTargetTrades { get; set; }

        [Parameter("Recovery Multiplier", Group = "Strategy", DefaultValue = 1, MinValue = 1, Step = 0.5)]
        public double RecoveryMultiplier { get; set; }

        /// <summary>
        /// The broker should consider this value as maximum slip
        /// </summary>
        [Parameter("Slippage (pips)", Group = "Strategy", DefaultValue = 2.0, MinValue = 0.5, Step = 0.1)]
        public double SLIPPAGE { get; set; }

        /// <summary>
        /// Maximum spread allowed for trades
        /// </summary>
        [Parameter("Max Spread allowed", Group = "Filters", DefaultValue = 1.5, MinValue = 0.1, Step = 0.1)]
        public double SpreadToTrigger { get; set; }

        /// <summary>
        /// Time level expressed in double beyond which the cbot pauses
        /// </summary>
        [Parameter("Pause over this time", Group = "Filters", DefaultValue = 21.3, MinValue = 0, MaxValue = 23.59)]
        public double PauseOver { get; set; }

        /// <summary>
        /// Time level expressed in double below the cbot pauses
        /// </summary>
        [Parameter("Pause under this time", Group = "Filters", DefaultValue = 3, MinValue = 0, MaxValue = 23.59)]
        public double PauseUnder { get; set; }

        /// <summary>
        /// The maximum distance (GAP) in pips that can elapse between a close and an open (candlestick change)
        /// </summary>
        [Parameter("Max GAP Allowed (pips)", Group = "Filters", DefaultValue = 1, MinValue = 0, Step = 0.01)]
        public double GAP { get; set; }

        /// <summary>
        /// The maximum number of trades that the cbot must open
        /// </summary>
        [Parameter("Max Number of Trades", Group = "Filters", DefaultValue = 1, MinValue = 1, Step = 1)]
        public int MaxTrades { get; set; }

        /// <summary>
        /// Offers the ability to limit one-way strategies only
        /// </summary>
        [Parameter("Hedging Opportunity?", Group = "Filters", DefaultValue = false)]
        public bool HedgingOpportunity { get; set; }

        /// <summary>
        /// Exclusive value that bypasses the risk calculation, if equal to zero it does not take into account the manual value
        /// </summary>
        [Parameter("Fixed Lots", Group = "Money Management", DefaultValue = 0, MinValue = 0, Step = 0.01)]
        public double FixedLots { get; set; }

        /// <summary>
        /// The capital to be taken into account for the risk calculation
        /// </summary>
        [Parameter("Capital", Group = "Money Management", DefaultValue = Extensions.CapitalTo.Balance)]
        public Extensions.CapitalTo MyCapital { get; set; }

        /// <summary>
        /// The percentage of risk to be calculated for the batch size
        /// </summary>
        [Parameter("% Risk", Group = "Money Management", DefaultValue = 1, MinValue = 0.1, Step = 0.1)]
        public double MyRisk { get; set; }

        /// <summary>
        /// The number of pips to take into account if the Stop Loss is zero to calculate the size, if
        /// this value will also be zero then 100 will be set as the nominal value
        /// </summary>
        [Parameter("Pips To Calculate (if no stoploss, empty = '100')", Group = "Money Management", DefaultValue = 100, MinValue = 0, Step = 0.1)]
        public double FakeSL { get; set; }

        /// <summary>
        /// The Stop Loss that will be used for each operation
        /// </summary>
        [Parameter("Stop Loss (pips)", Group = "Standard Stop", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double SL { get; set; }

        /// <summary>
        /// The Take Profit that will be used for each trade
        /// </summary>
        [Parameter("Take Profit (pips)", Group = "Standard Stop", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double TP { get; set; }

        /// <summary>
        /// The number of periods to check to calculate the stoploss
        /// </summary>
        [Parameter("Period", Group = "Auto Stop", DefaultValue = 5, MinValue = 1, Step = 1)]
        public int AutoStopPeriod { get; set; }

        /// <summary>
        /// The minimum number of pips to consider
        /// </summary>
        [Parameter("Minimum (pips)", Group = "Auto Stop", DefaultValue = 10, MinValue = 1, Step = 0.1)]
        public double AutoMinPips { get; set; }

        /// <summary>
        /// The number of pips to add
        /// </summary>
        [Parameter("K (pips)", Group = "Auto Stop", DefaultValue = 3, MinValue = 0, Step = 0.1)]
        public double KPips { get; set; }

        /// <summary>
        /// The risk regard for the calculation of the take profit
        /// </summary>
        [Parameter("R: R (zero disable take profit)", Group = "Auto Stop", DefaultValue = 0, MinValue = 0, Step = 1)]
        public double AutoStopRR { get; set; }

        /// <summary>
        /// The activation for the monitoring of the Break Even for the negative logic
        /// </summary>
        [Parameter("Profit Direction?", Group = "Break Even", DefaultValue = Extensions.ProfitDirection.All)]
        public Extensions.ProfitDirection BreakEvenProfitDirection { get; set; }

        /// <summary>
        /// The activation for the monitoring of the Break Even, if equal to zero, it disables the control
        /// </summary>
        [Parameter("Activation (pips)", Group = "Break Even", DefaultValue = 30, MinValue = 1, Step = 0.1)]
        public double BreakEvenActivation { get; set; }

        /// <summary>
        /// The activation for the monitoring of the Break Even after a number of bars, if equal to zero, it disables the control
        /// </summary>
        [Parameter("Activation Limit (bars)", Group = "Break Even", DefaultValue = 11, MinValue = 0, Step = 1)]
        public int BreakEvenLimitBars { get; set; }

        /// <summary>
        /// The number of pips to move in case of activation of the Break Even, can be less than zero
        /// </summary>
        [Parameter("Distance (pips, move Stop Loss)", Group = "Break Even", DefaultValue = 1.5, Step = 0.1)]
        public double BreakEvenDistance { get; set; }

        /// <summary>
        /// The activation for the monitoring of the Trailing, if equal to zero, it disables the monitoring
        /// </summary>
        [Parameter("Activation (pips)", Group = "Trailing", DefaultValue = 40, MinValue = 1, Step = 0.1)]
        public double TrailingActivation { get; set; }

        /// <summary>
        /// Il numero di pips che segna la distanza del Trailing, se pari a zero inibisce il Trailing
        /// </summary>
        [Parameter("Distance (pips, move Stop Loss)", Group = "Trailing", DefaultValue = 30, MinValue = 1, Step = 0.1)]
        public double TrailingDistance { get; set; }

        /// <summary>
        /// Attiva la logica proattiva per l'accelerazione del trailing
        /// </summary>
        [Parameter("ProActive ?", Group = "Trailing", DefaultValue = false)]
        public bool TrailingProactive { get; set; }

        [Parameter("Capital", Group = "Drawdown", DefaultValue = AccCapital.Balance)]
        public AccCapital accCapital { get; set; }

        [Parameter("Max %", Group = "Drawdown", DefaultValue = 20, MinValue = 0, MaxValue = 100, Step = 0.1)]
        public double ddPercentage { get; set; }

        /// <summary>
        /// Opzione per il debug che apre una posizione di test (label TEST)
        /// </summary>
        [Parameter("Open Position On Start", Group = "Debug", DefaultValue = MyTradeType.Disabled)]
        public MyTradeType OpenOnStart { get; set; }

        /// <summary>
        /// Flag per visualizzare eventuali messaggi di debug
        /// </summary>
        [Parameter("Verbose ?", Group = "Debug", DefaultValue = true)]
        public bool DebugVerbose { get; set; }

        /// <summary>
        /// Il colore del testo per eventuali messaggi da stampare sul chart
        /// </summary>
        [Parameter("Color Text", Group = "Styles", DefaultValue = Extensions.ColorNameEnum.Coral)]
        public Extensions.ColorNameEnum TextColor { get; set; }

        #endregion

        #region Property

        Extensions.Monitor.PauseTimes Pause1;
        Extensions.Monitor Monitor1;
        Extensions.MonenyManagement MonenyManagement1;
        Extensions.Monitor.BreakEvenData BreakEvenData1;
        Extensions.Monitor.TrailingData TrailingData1;

        private double SafeLoss = 0;

        #endregion

        #region cBot Events

        /// <summary>
        /// Evento generato quando viene avviato il cBot
        /// </summary>
        protected override void OnStart()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            SafeLoss = (MyStopType == StopMode.Auto || SL > 0) ? StopLevel : 0;

            // --> Messaggio di avvertimento nel caso incui si eseguisse senza modifiche logiche
            if (_canDraw())
                Chart.DrawStaticText(NAME, "ATTENTION : CBOT BASE, EDIT THIS TEMPLATE ONLY", VerticalAlignment.Top, HorizontalAlignment.Left, Extensions.ColorFromEnum(TextColor));

            // --> Determino il range di pausa
            Pause1 = new Extensions.Monitor.PauseTimes
            {

                Over = PauseOver,
                Under = PauseUnder

            };

            // --> Inizializzo il Monitor
            Monitor1 = new Extensions.Monitor(MyLabel, Symbol, Bars, Positions, Pause1);

            // --> Inizializzo il MoneyManagement
            MonenyManagement1 = new Extensions.MonenyManagement(Account, MyCapital, MyRisk, FixedLots, SL > 0 ? SL : FakeSL, Symbol);

            // --> Inizializzo i dati per la gestione del breakeven
            BreakEvenData1 = new Extensions.Monitor.BreakEvenData
            {

                OnlyFirst = BreakEvenProtectionType == ProtectionType.OnlyFirst,
                ProfitDirection = BreakEvenProfitDirection,
                Activation = (BreakEvenProtectionType != ProtectionType.Disabled) ? BreakEvenActivation : 0,
                LimitBar = BreakEvenLimitBars,
                Distance = BreakEvenDistance

            };

            // --> Inizializzo i dati per la gestione del Trailing
            TrailingData1 = new Extensions.Monitor.TrailingData
            {

                OnlyFirst = TrailingProtectionType == ProtectionType.OnlyFirst,
                ProActive = TrailingProactive,
                Activation = (TrailingProtectionType != ProtectionType.Disabled) ? TrailingActivation : 0,
                Distance = TrailingDistance

            };

            // --> Osservo le aperture per operazioni comuni
            Positions.Opened += _onOpenPositions;

            // --> Effettuo un test di apertura per verificare il funzionamento del sistema
            if (OpenOnStart != MyTradeType.Disabled)
                _test((OpenOnStart == MyTradeType.Buy) ? TradeType.Buy : TradeType.Sell, Monitor1, MonenyManagement1, MyLabel);

        }

        /// <summary>
        /// Evento generato quando viene fermato il cBot
        /// </summary>
        protected override void OnStop()
        {

            // --> Meglio eliminare l'handler, non dovrebbe servire ma non si sa mai
            Positions.Opened -= _onOpenPositions;

        }

        /// <summary>
        /// Evento generato ad ogni cambio candela
        /// </summary>
        protected override void OnBar()
        {

            // --> Resetto il flag del controllo candela
            Monitor1.OpenedInThisBar = false;

            // --> Eseguo il loop solo se desidero farlo ad ogni cambio candela
            if (MyLoopType == LoopType.OnBar)
                _loop(Monitor1, MonenyManagement1, BreakEvenData1, TrailingData1);

        }

        /// <summary>
        /// Evento generato a ogni tick
        /// </summary>
        protected override void OnTick()
        {

            // --> Devo comunque controllare i breakeven e altro nel tick
            Monitor1.Update(_checkClosePositions(Monitor1), Monitor1.Info.IAmInHedging ? null : BreakEvenData1, Monitor1.Info.IAmInHedging ? null : TrailingData1, SafeLoss, null);

            // --> Controllo il drawdown o se sono di nuovo in hedging
            if (Monitor1.Info.IAmInHedging || _checkDrawdownMode(Monitor1))
                return;

            // --> Controllo se sono passate troppe candele e voglio chiudere, minimo 2 posizioni
            if (Boring > 0 && Monitor1.Positions.Length >= 2 && Monitor1.Info.TotalNetProfit > 0)
            {

                // --> Ricavo l'indice della prima posizione
                int currentIndex = Bars.Count - 1;
                int indexFirstTrade = Bars.OpenTimes.GetIndexByTime(Monitor1.Info.FirstPosition.EntryTime);

                if ((currentIndex - indexFirstTrade) >= Boring)
                {

                    Monitor1.CloseAllPositions();
                    _log("Closed for Boring Bars ");
                    return;
                }

            }

            // --> Eseguo il loop solo se desidero farlo ad ogni Tick
            if (MyLoopType == LoopType.OnTick)
                _loop(Monitor1, MonenyManagement1, BreakEvenData1, TrailingData1);

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Operazioni da compiere ogni volta che apro una posizione con questa label
        /// </summary>
        private void _onOpenPositions(PositionOpenedEventArgs eventArgs)
        {

            if (eventArgs.Position.SymbolName == Monitor1.Symbol.Name && eventArgs.Position.Label == Monitor1.Label)
            {

                Monitor1.OpenedInThisBar = true;
                Monitor1.OpenedInThisTrigger = true;

            }

        }

        private void _loop(Extensions.Monitor monitor, Extensions.MonenyManagement moneymanagement, Extensions.Monitor.BreakEvenData breakevendata, Extensions.Monitor.TrailingData trailingdata)
        {

            // --> Controllo se ho il consenso a procedere con i trigger
            _checkResetTrigger(monitor);

            // --> Condizione condivisa, filtri generali, segnano il perimetro di azione limitando l'ingresso
            bool sharedCondition = (_canCowork(monitor) && !monitor.OpenedInThisBar && !monitor.OpenedInThisTrigger && !monitor.InGAP(GAP) && !monitor.InPause(Server.Time) && monitor.Symbol.RealSpread() <= SpreadToTrigger && monitor.Positions.Length < MaxTrades);

            // --> Controllo la presenza di trigger d'ingresso tenendo conto i filtri
            bool triggerBuy = _calculateLongTrigger(_calculateLongFilter(sharedCondition));
            bool triggerSell = _calculateShortTrigger(_calculateShortFilter(sharedCondition));

            // --> Se ho entrambi i trigger qualcosa non va, lo segnalo nei log e fermo la routin
            if (triggerBuy && triggerSell)
            {

                Print("{0} {1} ERROR : trigger buy and sell !", monitor.Label, monitor.Symbol.Name);
                return;

            }

            // --> Calcolo la size in base al money management stabilito, ma prima devo resettare la misura del calcolo
            moneymanagement.PipToCalc = SL;
            double lotSize = (monitor.Info.TotalNetProfit < 0 && RecoveryMultiplier > 1 && monitor.Info.MaxVolumeInUnits > 0) ? Math.Round(monitor.Symbol.VolumeInUnitsToQuantity(monitor.Info.MaxVolumeInUnits) * RecoveryMultiplier, 2) : moneymanagement.GetLotSize();

            double volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(lotSize);

            double tmpSL = SL;
            double tmpTP = TP;

            // --> Se ho il segnale d'ingresso considerando i filtri allora procedo con l'ordine a mercato
            if (MyOpenTradeType != Extensions.OpenTradeType.Sell && triggerBuy)
            {

                // --> Devo dimensionare lo stop
                if (MyStopType == StopMode.Auto)
                {

                    double lowest = monitor.Bars.LowPrices.Minimum(AutoStopPeriod);
                    tmpSL = monitor.Symbol.DigitsToPips(monitor.Symbol.Ask - lowest);
                    tmpSL += KPips;

                    if (tmpSL < AutoMinPips)
                        tmpSL = AutoMinPips;
                    tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                    moneymanagement.PipToCalc = tmpSL;
                    lotSize = (monitor.Info.TotalNetProfit < 0 && RecoveryMultiplier > 1 && monitor.Info.MaxVolumeInUnits > 0) ? Math.Round(monitor.Symbol.VolumeInUnitsToQuantity(monitor.Info.MaxVolumeInUnits) * RecoveryMultiplier, 2) : moneymanagement.GetLotSize();

                    volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(lotSize);

                }

                ExecuteMarketRangeOrder(TradeType.Buy, monitor.Symbol.Name, volumeInUnits, SLIPPAGE, monitor.Symbol.Ask, monitor.Label, tmpSL, tmpTP);

            }
            else if (MyOpenTradeType != Extensions.OpenTradeType.Buy && triggerSell)
            {

                // --> Devo dimensionare lo stop
                if (MyStopType == StopMode.Auto)
                {

                    double highest = monitor.Bars.HighPrices.Maximum(AutoStopPeriod);
                    tmpSL = monitor.Symbol.DigitsToPips(highest - monitor.Symbol.Bid);
                    tmpSL += KPips;

                    if (tmpSL < AutoMinPips)
                        tmpSL = AutoMinPips;
                    tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                    moneymanagement.PipToCalc = tmpSL;
                    lotSize = (monitor.Info.TotalNetProfit < 0 && RecoveryMultiplier > 1 && monitor.Info.MaxVolumeInUnits > 0) ? Math.Round(monitor.Symbol.VolumeInUnitsToQuantity(monitor.Info.MaxVolumeInUnits) * RecoveryMultiplier, 2) : moneymanagement.GetLotSize();

                    volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(lotSize);

                }

                ExecuteMarketRangeOrder(TradeType.Sell, monitor.Symbol.Name, volumeInUnits, SLIPPAGE, monitor.Symbol.Bid, monitor.Label, tmpSL, tmpTP);

            }

        }

        #endregion

        #region Strategy

        /// <summary>
        /// Controlla la logica del trigger e ne resetta lo stato
        /// </summary>
        /// <param name="monitor"></param>
        private void _checkResetTrigger(Extensions.Monitor monitor)
        {

            /*

    Bisogna sfruttare questo reset per impedire di aprire posizioni inutilmente, immaginate
    un ingresso quando il trend è fortemente direzionale, in tal caso se fossimo controtrend
    sarebbe un disastro quindi con questo flag si aspetta che il trend sia finito per tentare
    di accedere contro trend di nuovo.

 */
            monitor.OpenedInThisTrigger = false;

        }

        /// <summary>
        /// Controlla e stabilisce se si devono chiudere tutte le posizioni
        /// </summary>
        private bool _checkClosePositions(Extensions.Monitor monitor)
        {

            // --> Criteri da stabilire con la strategia, monitor.Positions......
            bool numtargets = monitor.Positions.Length >= MoneyTargetTrades;

            double realmoneytarget = Math.Round((Account.Balance / 100) * MoneyTargetPercentage, monitor.Symbol.Digits);

            return (numtargets && realmoneytarget > 0 && monitor.Info.TotalNetProfit >= realmoneytarget);

        }

        /// <summary>
        /// Conferma se i criteri di filtraggio long sono stati soddisfatti
        /// </summary>
        /// <param name="condition">Filtro globale, condizione condivisa</param>
        /// <returns>I filtri long sono stati soddisfatti</returns>
        private bool _calculateLongFilter(bool condition = true)
        {

            // --> La condizione primaria deve essere presente altrimenti non serve continuare
            if (!condition)
                return false;

            // --> In caso di multi-operations non posso andare in hedging, a patto che non venga scelto esplicitamente
            if (!HedgingOpportunity && Monitor1.Info.SellPositions > 0)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return true;

        }

        /// <summary>
        /// Conferma se i criteri di filtraggio short sono stati soddisfatti
        /// </summary>
        /// <param name="condition">Filtro globale, condizione condivisa</param>
        /// <returns>I filtri short sono stati soddisfatti</returns>
        private bool _calculateShortFilter(bool condition = true)
        {

            // --> La condizione primaria deve essere presente altrimenti non serve continuare
            if (!condition)
                return false;

            // --> In caso di multi-operations non posso andare in hedging, a patto che non venga scelto esplicitamente
            if (!HedgingOpportunity && Monitor1.Info.BuyPositions > 0)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return true;

        }

        /// <summary>
        /// Conferma se i criteri d'ingresso long sono stati soddisfatti
        /// </summary>
        /// <param name="filter">Filtro long, condizione condivisa</param>
        /// <returns>É presente una condizione di apertura long</returns>
        private bool _calculateLongTrigger(bool filter = true)
        {

            // --> Il filtro primario deve essere presente altrimenti non serve continuare
            if (!filter)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return false;

        }

        /// <summary>
        /// Conferma se i criteri d'ingresso short sono stati soddisfatti
        /// </summary>
        /// <param name="filter">Filtro short, condizione condivisa</param>
        /// <returns>É presente una condizione di apertura short</returns>
        private bool _calculateShortTrigger(bool filter = true)
        {

            // --> Il filtro primario deve essere presente altrimenti non serve continuare
            if (!filter)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return false;

        }

        /// <summary>
        /// Restituisce le coppie attualmente a lavoro
        /// </summary>
        /// <returns></returns>
        private List<string> _getOtherCross()
        {

            List<string> OtherCross = new List<string>();

            foreach (Position position in Positions)
            {

                if (position.SymbolName != SymbolName && !OtherCross.Contains(position.SymbolName))
                    OtherCross.Add(position.SymbolName);

            }

            return OtherCross;

        }

        /// <summary>
        /// Flag che ci autorizza a lavorare nel contesto, se ho già posizioni apro
        /// </summary>
        /// <returns></returns>
        private bool _canCowork(Extensions.Monitor monitor)
        {

            return (MaxCross == 0 || monitor.Positions.Length > 0) ? true : _getOtherCross().Count < MaxCross;

        }

        private bool _checkDrawdownMode(Extensions.Monitor monitor)
        {

            bool managed = false;

            if (ddMode == DrawDownMode.Disabled)
                return managed;

            // --> controllo se ho superato il limite per il DD
            double mmmDD = _currentDDMoney();

            if (monitor.Info.TotalNetProfit > mmmDD)
                return managed;

            // --> devo agire
            switch (ddMode)
            {

                case DrawDownMode.Close:

                    monitor.CloseAllPositions();

                    Print("{0} : Closed {1} all positions, hit max drawdown {2}", MyLabel, Symbol.Name, mmmDD);
                    managed = true;

                    break;

                case DrawDownMode.Hedging:

                    if (monitor.Info.TotalLotsBuy < monitor.Info.TotalLotsSell)
                    {

                        var volumeInUnits = Symbol.QuantityToVolumeInUnits(monitor.Info.TotalLotsSell - monitor.Info.TotalLotsBuy);

                        ExecuteMarketRangeOrder(TradeType.Buy, Symbol.Name, volumeInUnits, SLIPPAGE, Symbol.Ask, MyLabel, 0, 0);
                        monitor.OpenedInThisBar = true;
                        managed = true;

                        Print("{0} : Hedged {1} all positions, hit max drawdown {2}", MyLabel, Symbol.Name, mmmDD);

                    }
                    else if (monitor.Info.TotalLotsBuy > monitor.Info.TotalLotsSell)
                    {

                        var volumeInUnits = Symbol.QuantityToVolumeInUnits(monitor.Info.TotalLotsBuy - monitor.Info.TotalLotsSell);

                        ExecuteMarketRangeOrder(TradeType.Sell, Symbol.Name, volumeInUnits, SLIPPAGE, Symbol.Bid, MyLabel, 0, 0);
                        monitor.OpenedInThisBar = true;
                        managed = true;

                        Print("{0} : Hedged {1} all positions, hit max drawdown {2}", MyLabel, Symbol.Name, mmmDD);

                    }

                    break;

            }

            return managed;

        }

        private double _currentDDMoney()
        {

            double myCapital = Account.Balance;

            switch (accCapital)
            {

                case AccCapital.FreeMargin:

                    myCapital = Account.FreeMargin;
                    break;

                case AccCapital.Equity:

                    myCapital = Account.Equity;
                    break;

            }

            // --> Restituisco la percentuale negativa
            return (Math.Round((myCapital / 100) * ddPercentage, 2)) * -1;

        }

        private void _test(TradeType trigger, Extensions.Monitor monitor, Extensions.MonenyManagement moneymanagement, string label = "TEST")
        {

            if (!_canCowork(monitor))
            {

                _log("Can't Coworing!");
                return;

            }

            moneymanagement.PipToCalc = SL;
            double volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(moneymanagement.GetLotSize());

            double tmpSL = SL;
            double tmpTP = TP;

            switch (trigger)
            {

                case TradeType.Buy:

                    // --> Devo dimensionare lo stop
                    if (MyStopType == StopMode.Auto)
                    {

                        double lowest = monitor.Bars.LowPrices.Minimum(AutoStopPeriod);
                        tmpSL = monitor.Symbol.DigitsToPips(monitor.Symbol.Ask - lowest);
                        tmpSL += KPips;

                        if (tmpSL < AutoMinPips)
                            tmpSL = AutoMinPips;
                        tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                        moneymanagement.PipToCalc = tmpSL;
                        volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(moneymanagement.GetLotSize());

                    }

                    ExecuteMarketRangeOrder(TradeType.Buy, moneymanagement.Symbol.Name, volumeInUnits, SLIPPAGE, moneymanagement.Symbol.Ask, label, tmpSL, tmpTP);
                    break;

                case TradeType.Sell:

                    // --> Devo dimensionare lo stop
                    if (MyStopType == StopMode.Auto)
                    {

                        double highest = monitor.Bars.HighPrices.Maximum(AutoStopPeriod);
                        tmpSL = monitor.Symbol.DigitsToPips(highest - monitor.Symbol.Bid);
                        tmpSL += KPips;

                        if (tmpSL < AutoMinPips)
                            tmpSL = AutoMinPips;
                        tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                        moneymanagement.PipToCalc = tmpSL;
                        volumeUnits = monitor.Symbol.QuantityToVolumeInUnits(moneymanagement.GetLotSize Method());

                    }

                    ExecuteMarketRangeOrder(TradeType.Sell, moneymanagement.Symbol.Name, volumeInUnits, SLIPPAGE, moneymanagement.Symbol.Bid, label, tmpSL, tmpTP);
                    break;

            }

        }

        /// <summary>
        /// Check if graphic operations are possible on the Chart, to be used before each call to the Chart
        /// </summary>
        private bool _canDraw()
        {

            return RunningMode == RunningMode.RealTime || RunningMode == RunningMode.VisualBacktesting;

        }

        private void _log(string text)
        {

            if (!DebugVerbose || text.Trim().Length == 0)
                return;

            Print("{0} : {1}", NAME, text.Trim());

        }
        #endregion

    }

}