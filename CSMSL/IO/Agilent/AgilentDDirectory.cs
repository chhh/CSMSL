﻿using Agilent.MassSpectrometry.DataAnalysis;

namespace CSMSL.IO.Agilent
{
    public class AgilentDDirectory : MsDataFile
    {
        private IMsdrDataReader _msdr;

        public AgilentDDirectory(string directoryPath, bool openImmediately = false)
            : base(directoryPath, MsDataFileType.AgilentRawFile)
        {
            if(openImmediately) Open();
        }

        public override int FirstSpectrumNumber
        {
            get
            {
                if(base.FirstSpectrumNumber < 0)
                {
                    base.FirstSpectrumNumber = GetFirstSpectrumNumber();
                }
                return base.FirstSpectrumNumber;
            }
        }

        public override int LastSpectrumNumber
        {
            get
            {
                if(base.LastSpectrumNumber < 0)
                {
                    base.LastSpectrumNumber = GetLastSpectrumNumber();
                }
                return base.LastSpectrumNumber;
            }
        }

        public override void Open()
        {
            if(!IsOpen)
            {
                _msdr = (IMsdrDataReader)new MassSpecDataReader();
                _msdr.OpenDataFile(FilePath);
                base.Open();
            }
        }

        public override void Dispose()
        {
            if(_msdr != null)
            {
                _msdr.CloseDataFile();
                _msdr = null;
            }
            base.Dispose();
        }

        private int GetFirstSpectrumNumber()
        {
            return 1;
        }

        private int GetLastSpectrumNumber()
        {
            return (int)(_msdr.MSScanFileInformation.TotalScansPresent + 1);
        }

        public override double GetRetentionTime(int spectrumNumber)
        {
            IMSScanRecord scan_record = _msdr.GetScanRecord(spectrumNumber - 1);
            return scan_record.RetentionTime;
        }

        public override int GetMsnOrder(int spectrumNumber)
        {
            IMSScanRecord scan_record = _msdr.GetScanRecord(spectrumNumber - 1);
            return scan_record.MSLevel == MSLevel.MSMS ? 2 : 1;
        }

        private object GetExtraValue(int spectrumNumber, string filter)
        {
            IBDAActualData[] actuals = _msdr.ActualsInformation.GetActualCollection(GetRetentionTime(spectrumNumber));
            foreach(IBDAActualData actual in actuals)
            {
                if(actual.DisplayName == filter)
                {
                    return actual.DisplayValue;
                }
            }
            return null;
        }

        public override Polarity GetPolarity(int spectrumNumber)
        {
            IMSScanRecord scan_record = _msdr.GetScanRecord(spectrumNumber - 1);
            switch(scan_record.IonPolarity)
            {
                case IonPolarity.Positive:
                    return Polarity.Positive;
                case IonPolarity.Negative:
                    return Polarity.Negative;
                default:
                    return Polarity.Neutral;
            }
        }
    }
}