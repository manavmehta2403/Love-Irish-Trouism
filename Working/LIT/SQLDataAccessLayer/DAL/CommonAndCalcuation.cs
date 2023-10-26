using SQLDataAccessLayer.SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SQLDataAccessLayer.Models;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using LITModels;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using LITModels.LITModels.Models;

namespace SQLDataAccessLayer.DAL
{
    public class CommonAndCalcuation
    {
        Errorlog errobj = new Errorlog();
        DBConnectionEF DBconnEF = new DBConnectionEF();
        public string LoadPaymentDueDate(string Purpose)
        {

            string retnstr = string.Empty;
            object objfolder = new object();
            SqlHelper.parameters = null;
            try
            {
                SqlHelper.inputparams("@Action", 100, Purpose, SqlDbType.VarChar);
                objfolder = SqlHelper.ExecuteScalar(DBConfiguration.instance.ConnectionString, "SP_LoadCommonValues", SqlHelper.parameters);
                if (objfolder != null)
                {
                    return objfolder.ToString();
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("CommonAndCalcuation", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return retnstr;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }


        public List<CurrencySettingvalues> LoadCurrencySettingsValues(string bkcurrencyid, string itincurrencyid)
        {

            List<CurrencySettingvalues> CurrencySettingValueLists = new List<CurrencySettingvalues>();
            SqlHelper.parameters = null;
            string spname = string.Empty;
            try
            {

                SqlHelper.inputparams("@FromCurrencyID", 100, Guid.Parse(bkcurrencyid), SqlDbType.UniqueIdentifier);
                SqlHelper.inputparams("@ToCurrencyID", 100, Guid.Parse(itincurrencyid), SqlDbType.UniqueIdentifier);

                using (SqlDataReader rdr = SqlHelper.ExecuteReader(DBConfiguration.instance.ConnectionString, "GetCurrencySettingValues", SqlHelper.parameters))
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            CurrencySettingvalues CursetObj = new CurrencySettingvalues();
                            CursetObj.FromCurrencyId = ((rdr["FromCurrencyID"] == DBNull.Value) ? Guid.Empty : (Guid)(rdr["FromCurrencyID"]));
                            CursetObj.FromCurrencyValue = (rdr["FromCurrencyValue"] == DBNull.Value) ? 0 : (decimal)rdr["FromCurrencyValue"];
                            CursetObj.ToCurrencyId = (rdr["ToCurrencyID"] == DBNull.Value) ? Guid.Empty : (Guid)rdr["ToCurrencyID"];
                            CursetObj.ToCurrencyValue = (rdr["ToCurrencyValue"] == DBNull.Value) ? 0 : (decimal)rdr["ToCurrencyValue"];
                            CurrencySettingValueLists.Add(CursetObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("CommonAndCalcuation", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return CurrencySettingValueLists;//GetDbvalue(Purpose, "SP_LoadCommonValues");

        }

        public Tuple<decimal, decimal> CalculateItinearycurrency(string ItinCurrencyID, string BkgCurrencyID)
        {
            decimal ItinCurr = 0;
            Tuple<decimal, decimal> tu = null;
            List<CurrencySettingvalues> Listset = new List<CurrencySettingvalues>();
            decimal? fromcurr_Bkgcurr = 0, tocurr_itincurr = 0;
            try
            {
                Listset = LoadCurrencySettingsValues(BkgCurrencyID, ItinCurrencyID);

                if (Listset.Count > 0)
                {
                    fromcurr_Bkgcurr = Listset[0].FromCurrencyValue;
                    tocurr_itincurr = Listset[0].ToCurrencyValue;

                    ItinCurr = (decimal)(tocurr_itincurr / fromcurr_Bkgcurr);
                    tu = new Tuple<decimal, decimal>((decimal)fromcurr_Bkgcurr, ItinCurr);
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("CommonAndCalcuation", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return tu;

        }

        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();


        List<SupplierPriceRateEdit> ListofPricedit = new List<SupplierPriceRateEdit>();
        // Errorlog errobj = new Errorlog();


        private ObservableCollection<SupplierPriceRateEdit> _PriceEditDt;
        public ObservableCollection<SupplierPriceRateEdit> PriceEditDt
        {
            get { return _PriceEditDt ?? (_PriceEditDt = new ObservableCollection<SupplierPriceRateEdit>()); }
            set
            {
                _PriceEditDt = value;
            }
        }
        public void ReterivePricingEditRateWithDeleted(string SupplierServiceId, string PricingOptionId)
        {
            try
            {
                // SupplierPriceRateEdit sspredobj = dgPricingRateEdit.SelectedItem as SupplierPriceRateEdit;
                if (PriceEditDt != null)
                {
                    ListofPricedit = null;
                    PriceEditDt = null;
                    ListofPricedit = objsupdal.PriceEditRateRetrive(Guid.Parse(SupplierServiceId),
                         Guid.Parse(PricingOptionId), true);
                    if (ListofPricedit != null && ListofPricedit.Count > 0)
                    {
                        foreach (SupplierPriceRateEdit sup in ListofPricedit)
                        {
                            if (PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId && x.PriceEditRateId == sup.PriceEditRateId).Count() == 0)
                            {
                                PriceEditDt.Add(sup);
                            }
                        }
                    }
                    if (ListofPricedit == null || ListofPricedit.Count == 0)
                    {
                        // dgPricingRateEdit.ItemsSource = null;
                    }
                    // dgPricingRateEdit.ItemsSource = PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList();
                }


            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SuppPricingOptionTemplate", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        public void Grossnetcalculation(BookingItems objbkt)
        {
            decimal Grossvalue = 0, netvalue = 0;
            string markuppercen = string.Empty, commissionpercen = string.Empty;
            int cnt = Convert.ToInt32(objbkt.NtsDays);
            string dayofweek = string.Empty;
            decimal grossprice = 0; decimal netprice = 0; decimal grosspricetotal = 0; decimal netpricetotal = 0;
            string strgrossvalue = string.Empty;
            string strnetvalue = string.Empty;
            string[] strgrossvaluearr = new string[cnt];
            string[] strnetvaluearr = new string[cnt];
            if (objbkt.NewNetUnitNotinSupptbl == true)
            {
                grossprice = Convert.ToDecimal((objbkt.NewGrossunit != null) ? objbkt.NewGrossunit : objbkt.Grossunit);
                netprice = Convert.ToDecimal((objbkt.NewNetunit != null) ? objbkt.NewNetunit : objbkt.Netunit);
                objbkt.Grossunit = grossprice.ToString("0.00");
                // objbkt.GrossAdj = Grossvalue;                 
                objbkt.Grossfinal = grossprice * objbkt.Qty * objbkt.NtsDays;
                objbkt.Grosstotal = grossprice * objbkt.Qty * objbkt.NtsDays;
                objbkt.GrossAdj = ((decimal)DBconnEF.GrossAdjCalculation(objbkt.Grossfinal));

                objbkt.Netfinal = netprice * objbkt.Qty * objbkt.NtsDays;
                objbkt.Nettotal = netprice * objbkt.Qty * objbkt.NtsDays;

                objbkt.Netunit = netprice.ToString("0.00");
            }
            else
            {
                ReterivePricingEditRateWithDeleted(objbkt.ServiceID, objbkt.PricingOptionId);

                List<SupplierServiceDetailsRate> lstserdtrate = DBconnEF.ValidServiceRatesBtwDates(objbkt.StartDate, objbkt.NtsDays.ToString(), Guid.Parse(objbkt.ServiceID), Guid.Parse(objbkt.PricingRateID));
                if (lstserdtrate.Count > 0)
                {
                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId).Count() > 0)
                    {

                        dayofweek = ((objbkt.StartDate).DayOfWeek).ToString();
                        for (int i = 0; i < cnt; i++)
                        {

                            switch (dayofweek.ToString().ToLower())
                            {

                                case "sunday":
                                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault() != null)
                                    {
                                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault().GrossPrice;
                                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault().NetPrice;
                                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
                                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
                                        strgrossvaluearr[i] = grossprice.ToString("0.00");
                                        strnetvaluearr[i] = netprice.ToString("0.00");
                                    }
                                    break;
                                case "monday":
                                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault() != null)
                                    {
                                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault().GrossPrice;
                                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault().NetPrice;
                                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
                                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
                                        strgrossvaluearr[i] = grossprice.ToString("0.00");
                                        strnetvaluearr[i] = netprice.ToString("0.00");
                                    }
                                    break;
                                case "tuesday":
                                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault() != null)
                                    {
                                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault().GrossPrice;
                                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault().NetPrice;
                                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
                                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
                                        strgrossvaluearr[i] = grossprice.ToString("0.00");
                                        strnetvaluearr[i] = netprice.ToString("0.00");
                                    }
                                    break;
                                case "wednesday":
                                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault() != null)
                                    {
                                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault().GrossPrice;
                                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault().NetPrice;
                                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
                                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
                                        strgrossvaluearr[i] = grossprice.ToString("0.00");
                                        strnetvaluearr[i] = netprice.ToString("0.00");
                                    }
                                    break;
                                case "thursday":
                                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault() != null)
                                    {
                                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault().GrossPrice;
                                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault().NetPrice;
                                        //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
                                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
                                        strgrossvaluearr[i] = grossprice.ToString("0.00");
                                        strnetvaluearr[i] = netprice.ToString("0.00");
                                    }
                                    break;
                                case "friday":
                                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault() != null)
                                    {
                                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault().GrossPrice;
                                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault().NetPrice;
                                        //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
                                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
                                        strgrossvaluearr[i] = grossprice.ToString("0.00");
                                        strnetvaluearr[i] = netprice.ToString("0.00");
                                    }
                                    break;
                                case "saturday":
                                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault() != null)
                                    {
                                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault().GrossPrice;
                                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault().NetPrice;
                                        //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
                                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
                                        strgrossvaluearr[i] = grossprice.ToString("0.00");
                                        strnetvaluearr[i] = netprice.ToString("0.00");
                                    }
                                    break;
                            }
                            dayofweek = ((objbkt.StartDate.AddDays(i + 1)).DayOfWeek).ToString();
                        }

                    }
                    objbkt.Grossunit = (strgrossvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strgrossvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strgrossvaluearr.Distinct().OfType<string>().ToArray());
                    //objbkt.GrossAdj = Grossvalue;                    
                    objbkt.Grossfinal = grosspricetotal;//Grossvalue * objBI.Qty* objBI.NtsDays;
                    objbkt.Grosstotal = grosspricetotal;//Grossvalue * objBI.Qty * objBI.NtsDays;
                    objbkt.GrossAdj = ((decimal)DBconnEF.GrossAdjCalculation(objbkt.Grossfinal));

                    objbkt.Netfinal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 
                    objbkt.Nettotal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 

                    objbkt.Netunit = (strnetvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strnetvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strnetvaluearr.Distinct().OfType<string>().ToArray());
                }
                else
                {
                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId).FirstOrDefault().GrossPrice;
                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId).FirstOrDefault().NetPrice;
                    objbkt.Grossunit = grossprice.ToString("0.00");
                    //objbkt.GrossAdj = Grossvalue;
                    
                    objbkt.Grossfinal = grossprice * objbkt.Qty * objbkt.NtsDays;
                    objbkt.Grosstotal = grossprice * objbkt.Qty * objbkt.NtsDays;
                    objbkt.GrossAdj = ((decimal)DBconnEF.GrossAdjCalculation(objbkt.Grossfinal));

                    objbkt.Netfinal = netprice * objbkt.Qty * objbkt.NtsDays;
                    objbkt.Nettotal = netprice * objbkt.Qty * objbkt.NtsDays;

                    objbkt.Netunit = netprice.ToString("0.00");
                }
            }

            Tuple<decimal, decimal> curr = CalculateItinearycurrency(objbkt.ItinCurrencyID, objbkt.BkgCurrencyID);
            if (curr != null)
            {
                if (objbkt.ItinCurrency != objbkt.BkgCurrencyName && objbkt.ItinCurrencyID != objbkt.BkgCurrencyID)
                {
                    if (objbkt.Grosstotal > 0 && objbkt.Nettotal > 0 && curr.Item1 > 0 && curr.Item2 > 0)
                    {
                        objbkt.BkgNettotal = objbkt.Nettotal;
                        objbkt.BkgNetfinal = objbkt.Netfinal;
                        objbkt.BkgGrosstotal = objbkt.Grosstotal;
                        objbkt.BkgGrossfinal = objbkt.Grossfinal;

                        objbkt.Nettotal = curr.Item2 * objbkt.Nettotal;
                        objbkt.Netfinal = curr.Item2 * objbkt.Netfinal;
                        objbkt.Grosstotal = curr.Item2 * objbkt.Grosstotal;
                        objbkt.Grossfinal = curr.Item2 * objbkt.Grossfinal;

                        objbkt.ChangeCurrencyID = objbkt.BkgCurrencyID;


                    }
                }
                else
                {
                    objbkt.BkgNettotal = curr.Item1 * objbkt.Nettotal;
                    objbkt.BkgNetfinal = curr.Item1 * objbkt.Netfinal;
                    objbkt.BkgGrosstotal = curr.Item1 * objbkt.Grosstotal;
                    objbkt.BkgGrossfinal = curr.Item1 * objbkt.Grossfinal;
                    objbkt.ChangeCurrencyID = objbkt.ItinCurrencyID;
                }
            }
            else
            {
                objbkt.BkgNettotal = objbkt.Nettotal;
                objbkt.BkgNetfinal = objbkt.Netfinal;
                objbkt.BkgGrosstotal = objbkt.Grosstotal;
                objbkt.BkgGrossfinal = objbkt.Grossfinal;
                objbkt.ChangeCurrencyID = objbkt.ItinCurrencyID;
            }
            // LoadBookingEditGrid(objbkt.BookingID);

        }


        public void NewRatesforSelectedDates(BookingItems objbkt)
        {
            decimal netprice = 0;
            int cnt = Convert.ToInt32(objbkt.NtsDays);
            string dayofweek = string.Empty;
            string[] strnetvaluearr = new string[cnt];
            ReterivePricingEditRateWithDeleted(objbkt.ServiceID, objbkt.PricingOptionId);
            List<SupplierServiceDetailsRate> lstserdtrate = DBconnEF.ValidServiceRatesBtwDates(objbkt.StartDate, objbkt.NtsDays.ToString(), Guid.Parse(objbkt.ServiceID), Guid.Parse(objbkt.PricingRateID));
            if (lstserdtrate.Count > 0)
            {
                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId).Count() > 0)
                {
                    dayofweek = ((objbkt.StartDate).DayOfWeek).ToString();
                    for (int i = 0; i < cnt; i++)
                    {

                        switch (dayofweek.ToString().ToLower())
                        {

                            case "sunday":
                                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault() != null)
                                {
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault().NetPrice;
                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                }
                                break;
                            case "monday":
                                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault() != null)
                                {
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault().NetPrice;
                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                }
                                break;
                            case "tuesday":
                                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault() != null)
                                {
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault().NetPrice;
                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                }
                                break;
                            case "wednesday":
                                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault() != null)
                                {
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault().NetPrice;
                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                }
                                break;
                            case "thursday":
                                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault() != null)
                                {
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault().NetPrice;
                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                }
                                break;
                            case "friday":
                                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault() != null)
                                {
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault().NetPrice;
                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                }
                                break;
                            case "saturday":
                                if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault() != null)
                                {
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault().NetPrice;
                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                }
                                break;
                        }
                        dayofweek = ((objbkt.StartDate.AddDays(i + 1)).DayOfWeek).ToString();
                    }

                    objbkt.Netunit = (strnetvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strnetvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strnetvaluearr.Distinct().OfType<string>().ToArray());
                    objbkt.NewNetunit = (strnetvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strnetvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strnetvaluearr.Distinct().OfType<string>().ToArray());
                    Image SuccessImage = new Image();
                    BitmapImage SuccessImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "check"), UriKind.Relative));
                    SuccessImage.Source = SuccessImagebitmapImage;
                    objbkt.Resultmsgurl = SuccessImagebitmapImage;
                    objbkt.Resultmsg = "";

                }
                else
                {

                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId).FirstOrDefault().NetPrice;
                    objbkt.Netunit = netprice.ToString();
                    objbkt.NewNetunit = netprice.ToString();
                    objbkt.Resultmsg = "Update...";
                    Image FailureImage = new Image();
                    BitmapImage FailureImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "deticon"), UriKind.Relative));
                    FailureImage.Source = FailureImagebitmapImage;
                    objbkt.Resultmsgurl = FailureImagebitmapImage;
                }
            }
            else
            {
                if (PriceEditDt.Count == 0 && objbkt.NewNetUnitNotinSupptbl == true)
                {
                    Image SuccessImage = new Image();
                    BitmapImage SuccessImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "check"), UriKind.Relative));
                    SuccessImage.Source = SuccessImagebitmapImage;
                    objbkt.Resultmsgurl = SuccessImagebitmapImage;
                    objbkt.Resultmsg = "";

                }
                else
                {
                    netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId).FirstOrDefault().NetPrice;
                    objbkt.Netunit = netprice.ToString();
                    objbkt.NewNetunit = netprice.ToString();
                    objbkt.Resultmsg = "Update...";
                    Image FailureImage = new Image();
                    BitmapImage FailureImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "deticon"), UriKind.Relative));
                    FailureImage.Source = FailureImagebitmapImage;
                    objbkt.Resultmsgurl = FailureImagebitmapImage;
                }
            }
        }

        public Tuple<string, string> ReteriveWarnings(string SupplierServiceIdval, DateTime dtfrom, DateTime dtto)
        {
            Tuple<string, string> tupres = null;
            string servicewarning = string.Empty;
            string supplierwarning = string.Empty;
            int serwarcnt = 0;
            int supwarcnt = 0;
            List<SupplierServiceWarning> ListofSuppwarning = new List<SupplierServiceWarning>();
            try
            {
                if (SupplierServiceIdval != "")
                {

                    ListofSuppwarning = objsupdal.SupplServiceWarningdtRetrive(Guid.Parse(SupplierServiceIdval), dtfrom, dtto);
                    if (ListofSuppwarning != null && ListofSuppwarning.Count > 0)
                    {
                        // x => x.SupplierServiceDetailsWarningID == sup.SupplierServiceDetailsWarningID && (x.Messagefor.ToString().ToLower() == "supplier" || x.Messagefor.ToString().ToLower() == "service")
                        foreach (SupplierServiceWarning sup in ListofSuppwarning)
                        {
                            if (sup.Messagefor.ToString().ToLower() == "supplier")
                            {
                                if (!string.IsNullOrEmpty(sup.WarDescription))
                                {
                                    supwarcnt = supwarcnt + 1;
                                    supplierwarning += supwarcnt + ". [From " + sup.ValidFromwarning.ToString() + "] - [Until " + sup.ValidTowarning.ToString() + "] " + sup.WarDescription + "\n";
                                }
                            }
                            if (sup.Messagefor.ToString().ToLower() == "service")
                            {
                                if (!string.IsNullOrEmpty(sup.WarDescription))
                                {
                                    serwarcnt = serwarcnt + 1;
                                    servicewarning += serwarcnt + ". [From " + sup.ValidFromwarning.ToString() + "] - [Until " + sup.ValidTowarning.ToString() + "] " + sup.WarDescription + "\n";
                                }
                            }
                        }
                    }
                    tupres = new Tuple<string, string>(servicewarning, supplierwarning);

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("CommonAndCalcuation", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return tupres;
        }

        public List<FTPServerdetails> ReteriveFTPDetails()
        {
            FTPServerdetails ftpServdetails = new FTPServerdetails();
            List<FTPServerdetails> lstftp = new List<FTPServerdetails>();
            List<Setting> lstset = DBconnEF.ValidFTPServer();
            ftpServdetails.FTPUrl = lstset.Where(x => x.FieldName.ToLower() == "ftpurl").FirstOrDefault().FieldValue;
            ftpServdetails.FTPUsername = lstset.Where(x => x.FieldName.ToLower() == "ftpusername").FirstOrDefault().FieldValue;
            ftpServdetails.FTPPassword = lstset.Where(x => x.FieldName.ToLower() == "ftppassword").FirstOrDefault().FieldValue;
            lstftp.Add(ftpServdetails);
            return lstftp;
        }

        public static void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                /* bring the data item (Product object) into view
                 * in case it has been virtualized away */
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            //TODO: Retrieve and focus a DataGridCell object
        }
    }
}
