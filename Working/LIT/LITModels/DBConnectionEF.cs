using System;
using System.Linq;
using System.Collections.Generic;
using LITModels.LITModels.Models;

namespace LITModels
{
    public class DBConnectionEF
    {

        //String connstr = "Data Source=DB01;initial catalog=LoveIrishTour;user id=Loveirishtouruser;password=Loveirishtouruser";

        public List<CurrencySetting> CurrencySettingvalues()
        {
            List<CurrencySetting> ListCur = new List<CurrencySetting>();
            using (var context = new LoveIrishTourContext())
            {
                ListCur = context.CurrencySettings.ToList();

            }
            return ListCur;
        }
        public List<Currencydetail> Currencydispalyformat()
        {
            List<Currencydetail> ListCur = new List<Currencydetail>();
            using (var context = new LoveIrishTourContext())
            {
                ListCur = context.Currencydetails.ToList();

            }
            return ListCur;
        }

        public string ExistingItineraryStatus(string ItinID)
        {
            string existingItinStatus = string.Empty;
            using (var context = new LoveIrishTourContext())
            {
                existingItinStatus = (context.ItineraryDetails.Where(x => x.ItineraryId == Guid.Parse(ItinID)).FirstOrDefault() != null) ? context.ItineraryDetails.Where(x => x.ItineraryId == Guid.Parse(ItinID)).FirstOrDefault().Status.ToString() : string.Empty;
            }
            return existingItinStatus;
        }

        public string GetSupplierName(string SupplierId)
        {
            string SupplierName = string.Empty;
            using (var context = new LoveIrishTourContext())
            {
                SupplierName = (context.SupplierInformations.Where(x => x.SupplierId == Guid.Parse(SupplierId)).FirstOrDefault() != null) ? context.SupplierInformations.Where(x => x.SupplierId == Guid.Parse(SupplierId)).FirstOrDefault().SupplierName.ToString() : string.Empty;
            }
            return SupplierName;
        }

        public string GetMaxBookingid()
        {
            string bookingid = string.Empty;
            using (var context = new LoveIrishTourContext())
            {
                bookingid = (context.ItineraryBookings.Max(x => x.Bkid).ToString());
            }
            return bookingid;
        }

        public List<SupplierServiceDetailsRate> ValidServiceRatesBtwDates(DateTime StartDate, string NtsDays,
            Guid ServiceID, Guid PricingRateID)
        {
            List<SupplierServiceDetailsRate> LstSsdtRate = new List<SupplierServiceDetailsRate>();

            using (var context = new LoveIrishTourContext())
            {
                LstSsdtRate = (context.SupplierServiceDetailsRates.Where(x => x.ValidFrom <= StartDate && x.ValidTo >= (DateTime)(StartDate).AddDays(Convert.ToInt32(NtsDays))
                && x.SupplierServiceId == ServiceID && x.SupplierServiceDetailsRateId == PricingRateID)).ToList();
            }
            return LstSsdtRate;
        }

        public List<Setting> ValidFTPServer()
        {
            List<Setting> LstSettings = new List<Setting>();
            try
            {
                using (var context = new LoveIrishTourContext())
                {
                    LstSettings = context.Settings.Where(x => x.IsDeleted == false).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return LstSettings;
        }

        public bool Validateserviceavailable(Guid supplierid, Guid SupplierServiceid, Guid PricingRateID, Guid PricingOptionId, Guid PriceEditRateId)
        {
            Guid SupplierServiceDetailsRateIdrec = Guid.Empty, PricingOptionIdrec = Guid.Empty, Priceeditraterec = Guid.Empty, SupplierServiceidrec = Guid.Empty;


            using (var context = new LoveIrishTourContext())
            {
                SupplierServiceidrec = ((context.SupplierServices.Where(x => x.ServiceId == SupplierServiceid && x.SupplierId == supplierid)).Count() > 0) ?
                    (context.SupplierServices.Where(x => x.ServiceId == SupplierServiceid && x.SupplierId == supplierid)).FirstOrDefault().ServiceId : Guid.Empty;
                SupplierServiceDetailsRateIdrec = ((context.SupplierServiceDetailsRates.Where(x => x.SupplierServiceDetailsRateId == PricingRateID && x.SupplierServiceId == SupplierServiceid)).Count() > 0) ?
                    (context.SupplierServiceDetailsRates.Where(x => x.SupplierServiceDetailsRateId == PricingRateID && x.SupplierServiceId == SupplierServiceid)).FirstOrDefault().SupplierServiceDetailsRateId : Guid.Empty;
                PricingOptionIdrec = ((context.SupplierServicePricingoptions.Where(x => x.PricingOptionId == PricingOptionId && x.SupplierServiceId == SupplierServiceid)).Count() > 0) ?
                    (context.SupplierServicePricingoptions.Where(x => x.PricingOptionId == PricingOptionId && x.SupplierServiceId == SupplierServiceid)).FirstOrDefault().PricingOptionId : Guid.Empty;
                Priceeditraterec = ((context.SupplierServicePriceEditRates.Where(x => x.PriceEditRateId == PriceEditRateId && x.SupplierServiceId == SupplierServiceid)).Count() > 0) ?
                    (context.SupplierServicePriceEditRates.Where(x => x.PriceEditRateId == PriceEditRateId && x.SupplierServiceId == SupplierServiceid)).FirstOrDefault().PriceEditRateId : Guid.Empty;

                if (SupplierServiceidrec != Guid.Empty && SupplierServiceDetailsRateIdrec != Guid.Empty && PricingOptionIdrec != Guid.Empty && Priceeditraterec != Guid.Empty)
                {
                    return true;
                }

            }
            return false;
        }

        public decimal? GetSupplierMargin()
        {
            using (var context = new LoveIrishTourContext())
            {
                return ((context.SupplierMarginSettings.Count() > 0) ?
                    context.SupplierMarginSettings.FirstOrDefault().MarginValue : 0);
            }
        }

        public decimal? GetbookingMargin()
        {
            using (var context = new LoveIrishTourContext())
            {
                return ((context.MarginSettings.Count() > 0) ?
                    context.MarginSettings.FirstOrDefault().Overrideall : 0);
            }
        }
        public decimal? GrossAdjCalculation(decimal grossvalue, decimal Grossadjmarginval = 0, decimal GrossAdjmarkup = 0)
        {
            decimal? grossadj = 0, GrossAdjmargin = 0;
            if (Grossadjmarginval == 0)
            {
                GrossAdjmargin = GetbookingMargin();
            }
            else
            {
                GrossAdjmargin = (decimal)Grossadjmarginval;
            }
            if (GrossAdjmargin.HasValue)
            {
                grossadj = grossvalue * (1 + ((decimal)GrossAdjmargin.Value / 100));
            }
            if (GrossAdjmargin.HasValue && GrossAdjmarkup > 0)
            {
                grossadj = grossvalue * (((1 + ((decimal)GrossAdjmargin.Value / 100)) * (1 + ((decimal)GrossAdjmarkup / 100))));
            }
            return grossadj;
        }
        public decimal FinalMarginpercentagecalculation(decimal diff, decimal grossadj)
        {
            decimal finalmagper = 0;
            if (diff > 0 && grossadj > 0)
            {
                finalmagper = (diff / grossadj) * 100;
            }
            return finalmagper;

        }
        public decimal? GrossAdjMarkupCalculation(decimal grossvalue, decimal Grossadjmarginval = 0, decimal Markup = 0)
        {
            decimal? grossadj = 0, GrossAdjmargin = 0;
            if (Grossadjmarginval == 0)
            {
                GrossAdjmargin = GetbookingMargin();
            }
            else
            {
                GrossAdjmargin = (decimal)Grossadjmarginval;
            }
            if (GrossAdjmargin.HasValue)
            {
                if (Markup > 0)
                {
                    grossadj = grossvalue * (((decimal)GrossAdjmargin.Value / 100) * (Markup / 100));
                }
            }
            return grossadj;
        }

        public string GetReportArrowImage()
        {
            using (var context = new LoveIrishTourContext())
            {
                return ((context.Settings.Count() > 0) ?
                    context.Settings.Where(x => x.FieldName.ToLower() == "reportarrowimage").FirstOrDefault().FieldValue : string.Empty);
            }
        }

        public string GetServicetypename(string servicetypeid)
        {
            using (var context = new LoveIrishTourContext())
            {
                return ((context.ServiceTypes.Count() > 0) ?
                    context.ServiceTypes.Where(x => x.ServiceTypeId == Guid.Parse(servicetypeid)).FirstOrDefault().ServiceTypeName : string.Empty);
            }
        }

        public string GetCustomerpaymentlink()
        {
            using (var context = new LoveIrishTourContext())
            {
                return ((context.Settings.Count() > 0) ?
                    context.Settings.Where(x => x.FieldName.ToLower() == "CustomerPaymentlinkstrip").FirstOrDefault().FieldValue : string.Empty);
            }
        }

        public string GetImagePDFTHtmlFolderPath()
        {
            using (var context = new LoveIrishTourContext())
            {
                return ((context.Settings.Count() > 0) ?
                    context.Settings.Where(x => x.FieldName.ToLower() == "ImagePDFTHtmlFolder").FirstOrDefault().FieldValue : string.Empty);
            }
        }

    }
}
