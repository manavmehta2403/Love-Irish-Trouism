using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LITModels.LITModels.Models;

public partial class LoveIrishTourContext : DbContext
{
    public LoveIrishTourContext()
    {
    }

    public LoveIrishTourContext(DbContextOptions<LoveIrishTourContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgeGroup> AgeGroups { get; set; }

    public virtual DbSet<BookingNote> BookingNotes { get; set; }

    public virtual DbSet<BookingStatusRequest> BookingStatusRequests { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CommunicationNote> CommunicationNotes { get; set; }

    public virtual DbSet<CommunicationType> CommunicationTypes { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CurrencySetting> CurrencySettings { get; set; }

    public virtual DbSet<Currencydetail> Currencydetails { get; set; }

    public virtual DbSet<DropDownListHeader> DropDownListHeaders { get; set; }

    public virtual DbSet<DropDownListValueDetail> DropDownListValueDetails { get; set; }

    public virtual DbSet<EmailSetting> EmailSettings { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<FollowupTask> FollowupTasks { get; set; }

    public virtual DbSet<ItineraryAutoNumber> ItineraryAutoNumbers { get; set; }

    public virtual DbSet<ItineraryBooking> ItineraryBookings { get; set; }

    public virtual DbSet<ItineraryBookingTotal> ItineraryBookingTotals { get; set; }

    public virtual DbSet<ItineraryComment> ItineraryComments { get; set; }

    public virtual DbSet<ItineraryDetail> ItineraryDetails { get; set; }

    public virtual DbSet<ItineraryFolderinfo> ItineraryFolderinfos { get; set; }

    public virtual DbSet<MarginSetting> MarginSettings { get; set; }

    public virtual DbSet<OptionTypesforRoom> OptionTypesforRooms { get; set; }

    public virtual DbSet<PassengerDetail> PassengerDetails { get; set; }

    public virtual DbSet<PassengerGroup> PassengerGroups { get; set; }

    public virtual DbSet<PassengerType> PassengerTypes { get; set; }

    public virtual DbSet<Paxinformation> Paxinformations { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<Paymenttype> Paymenttypes { get; set; }

    public virtual DbSet<PickupDropLocation> PickupDropLocations { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<SupplierAutoNumber> SupplierAutoNumbers { get; set; }

    public virtual DbSet<SupplierCommunication> SupplierCommunications { get; set; }

    public virtual DbSet<SupplierEmailSetting> SupplierEmailSettings { get; set; }

    public virtual DbSet<SupplierInformation> SupplierInformations { get; set; }

    public virtual DbSet<SupplierMarginSetting> SupplierMarginSettings { get; set; }

    public virtual DbSet<SupplierPaymentsRecord> SupplierPaymentsRecords { get; set; }

    public virtual DbSet<SupplierService> SupplierServices { get; set; }

    public virtual DbSet<SupplierServiceDetailsRate> SupplierServiceDetailsRates { get; set; }

    public virtual DbSet<SupplierServiceDetailsWarning> SupplierServiceDetailsWarnings { get; set; }

    public virtual DbSet<SupplierServicePriceEditRate> SupplierServicePriceEditRates { get; set; }

    public virtual DbSet<SupplierServicePricingoption> SupplierServicePricingoptions { get; set; }

    public virtual DbSet<Supplierfolderinfo> Supplierfolderinfos { get; set; }

    public virtual DbSet<TestInsert> TestInserts { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connstring = ConfigurationManager.AppSettings["LITDBConnection"] != null ? ConfigurationManager.AppSettings["LITDBConnection"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(connstring))
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(connstring);
            }
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgeGroup>(entity =>
        {
            entity.HasKey(e => e.AgeGroupsid).HasName("PK__AgeGroup__17274C4A3C757E0A");

            entity.HasIndex(e => e.AgeGroupsname, "Indx_AgeGroupsname");

            entity.Property(e => e.AgeGroupsid).ValueGeneratedNever();
            entity.Property(e => e.AgeGroupsname)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BookingNote>(entity =>
        {
            entity.HasKey(e => e.BookingNotesid).HasName("PK__BookingN__1D6766684DAA03F5");

            entity.HasIndex(e => e.BookingId, "Indx_BookingId");

            entity.Property(e => e.Booking).HasMaxLength(2000);
            entity.Property(e => e.Privatemsg).HasMaxLength(2000);
            entity.Property(e => e.Voucher).HasMaxLength(2000);
        });

        modelBuilder.Entity<BookingStatusRequest>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("PK__BookingS__33AB555209A878E6");

            entity.ToTable("BookingStatusRequest");

            entity.HasIndex(e => e.BookingId, "Indx_BookingiId");

            entity.HasIndex(e => e.ItineraryId, "Indx_ItineraryId");

            entity.HasIndex(e => e.ReferenceNumber, "Indx_ReferenceNumber");

            entity.Property(e => e.Requestid).ValueGeneratedNever();
            entity.Property(e => e.AgencyEmail)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.AgencyName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BookingAutoId).HasColumnName("BookingAutoID");
            entity.Property(e => e.BookingidIdentifier)
                .IsUnicode(false)
                .HasColumnName("bookingidIdentifier");
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.IsEmailSent)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ItineraryAutoId)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ResponseStatus)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B76550DAB56");

            entity.ToTable("City");

            entity.HasIndex(e => e.RegionId, "Indx_RegionId");

            entity.Property(e => e.CityId).ValueGeneratedNever();
            entity.Property(e => e.CityName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CommunicationNote>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.SupplierId, "Indx_SupplierId");

            entity.Property(e => e.NoteId).HasColumnName("NoteID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
        });

        modelBuilder.Entity<CommunicationType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Communic__516F0395014F205C");

            entity.HasIndex(e => e.TypeName, "Indx_TypeName");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(200);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Country__10D1609F9C61E726");

            entity.ToTable("Country");

            entity.Property(e => e.CountryId).ValueGeneratedNever();
            entity.Property(e => e.CountryName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CurrencySetting>(entity =>
        {
            entity.HasKey(e => e.CurrencySettings).HasName("PK__Currency__FB47D9A9C52ACDDA");

            entity.HasIndex(e => e.FromCurrencyId, "Indx_FromCurrencyID");

            entity.HasIndex(e => e.ToCurrencyId, "Indx_ToCurrencyID");

            entity.Property(e => e.CurrencyFromDate).HasColumnType("date");
            entity.Property(e => e.CurrencyToDate).HasColumnType("date");
            entity.Property(e => e.FromCurrencyId).HasColumnName("FromCurrencyID");
            entity.Property(e => e.FromCurrencyValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ToCurrencyId).HasColumnName("ToCurrencyID");
            entity.Property(e => e.ToCurrencyValue).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Currencydetail>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__Currency__14470B10E589CE64");

            entity.HasIndex(e => new { e.CurrencyName, e.CurrencyCode }, "Indx_CurrencyCodeName");

            entity.Property(e => e.CurrencyId)
                .ValueGeneratedNever()
                .HasColumnName("CurrencyID");
            entity.Property(e => e.CurrencyCode).HasMaxLength(100);
            entity.Property(e => e.CurrencyCulture)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyName).HasMaxLength(500);
            entity.Property(e => e.DisplayFormat).HasMaxLength(100);
        });

        modelBuilder.Entity<DropDownListHeader>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DropDownListHeader");

            entity.Property(e => e.Descriptions).HasMaxLength(500);
            entity.Property(e => e.ListName).HasMaxLength(100);
        });

        modelBuilder.Entity<DropDownListValueDetail>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.DropdownlistHeaderId, "Indx_DropdownlistHeaderId");

            entity.Property(e => e.TextField).HasMaxLength(500);
        });

        modelBuilder.Entity<EmailSetting>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Bccemail)
                .HasMaxLength(200)
                .HasColumnName("BCCEmail");
            entity.Property(e => e.FromEmail).HasMaxLength(200);
            entity.Property(e => e.FromEmailName).HasMaxLength(200);
            entity.Property(e => e.FromEmailPassword).HasMaxLength(200);
            entity.Property(e => e.IsSslrequired).HasColumnName("IsSSLRequired");
            entity.Property(e => e.Smtpport).HasColumnName("SMTPPort");
            entity.Property(e => e.Smtpserver)
                .HasMaxLength(200)
                .HasColumnName("SMTPServer");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__ErrorLog__5E5499A82C54EE03");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ErrorFrom).HasMaxLength(500);
            entity.Property(e => e.FunctionName).HasMaxLength(500);
            entity.Property(e => e.PageName).HasMaxLength(500);
        });

        modelBuilder.Entity<FollowupTask>(entity =>
        {
            entity.HasKey(e => e.Taskid).HasName("PK__Followup__7C6DBDB90C1153BA");

            entity.ToTable("FollowupTask");

            entity.HasIndex(e => e.TaskName, "Indx_Taskname");

            entity.Property(e => e.Taskid).ValueGeneratedNever();
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.TaskName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ItineraryAutoNumber>(entity =>
        {
            entity.HasKey(e => e.Pkid).HasName("PK__Itinerar__A7DF3BD08E890033");

            entity.ToTable("ItineraryAutoNumber");
        });

        modelBuilder.Entity<ItineraryBooking>(entity =>
        {
            entity.HasKey(e => e.Bkid).HasName("PK__Itinerar__5235D3293F647F24");

            entity.ToTable("ItineraryBooking");

            entity.HasIndex(e => new { e.BookingAutoId, e.BookingName }, "Indx_BookingName_AutoID");

            entity.HasIndex(e => e.ItineraryId, "Indx_ItineraryId");

            entity.Property(e => e.AgentCommission).HasMaxLength(500);
            entity.Property(e => e.AgentCommissionPercentage).HasMaxLength(500);
            entity.Property(e => e.BkgCurrencyId).HasColumnName("BkgCurrencyID");
            entity.Property(e => e.BkgCurrencyName).HasMaxLength(500);
            entity.Property(e => e.BookingAutoId).HasColumnName("BookingAutoID");
            entity.Property(e => e.BookingName).HasMaxLength(500);
            entity.Property(e => e.BookingidIdentifier).HasColumnName("bookingidIdentifier");
            entity.Property(e => e.ChangeCurrencyId).HasColumnName("ChangeCurrencyID");
            entity.Property(e => e.City).HasMaxLength(200);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.CommissionPercentage).HasMaxLength(250);
            entity.Property(e => e.Day).HasMaxLength(100);
            entity.Property(e => e.DaysValid).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EndTime).HasMaxLength(250);
            entity.Property(e => e.ExchRate).HasMaxLength(250);
            entity.Property(e => e.GrossAdj).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Grossfinal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Grosstotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Grossunit).HasMaxLength(500);
            entity.Property(e => e.ItemDescription).HasMaxLength(1000);
            entity.Property(e => e.ItinCurrency).HasMaxLength(100);
            entity.Property(e => e.ItinCurrencyId).HasColumnName("ItinCurrencyID");
            entity.Property(e => e.MarkupPercentage).HasMaxLength(250);
            entity.Property(e => e.Netfinal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Nettotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Netunit).HasMaxLength(500);
            entity.Property(e => e.NightsDays).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.PaymentTerms).HasMaxLength(250);
            entity.Property(e => e.PricingRateId).HasColumnName("PricingRateID");
            entity.Property(e => e.Quantity).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Ref).HasMaxLength(250);
            entity.Property(e => e.Region).HasMaxLength(200);
            entity.Property(e => e.RegionId).HasColumnName("RegionID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ServiceName).HasMaxLength(250);
            entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");
            entity.Property(e => e.StartTime).HasMaxLength(250);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<ItineraryBookingTotal>(entity =>
        {
            entity.HasKey(e => e.ItineraryBookingTotalId).HasName("PK__Itinerar__22FE31DCAD603F82");

            entity.ToTable("ItineraryBookingTotal");

            entity.HasIndex(e => e.ItineraryId, "Indx_ItineararyId");

            entity.Property(e => e.ItineraryBookingTotalId).ValueGeneratedNever();
            entity.Property(e => e.FinalAgentCommission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FinalMargin).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FinalSell).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossAdjustment).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossAdjustmentFinalOverride).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossAdjustmentGross).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossAdjustmentMarkup).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossFinal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MarginAdjustmentGross).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MarginAdjustmentOverrideall).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetFinal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetTotal).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<ItineraryComment>(entity =>
        {
            entity.HasKey(e => e.Commentsid).HasName("PK__Itinerar__9498D2C4B3BCE517");

            entity.HasIndex(e => e.BookingId, "Indx_BookingId");

            entity.HasIndex(e => e.Itineraryid, "Indx_Itineraryid");

            entity.Property(e => e.Commentsid).ValueGeneratedNever();
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.SupplierRefNo)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ItineraryDetail>(entity =>
        {
            entity.HasKey(e => e.ItineraryId).HasName("PK__Itinerar__361216A640429B00");

            entity.HasIndex(e => new { e.ItineraryName, e.Email }, "Indx_ItineraryName_Email");

            entity.Property(e => e.ItineraryId)
                .ValueGeneratedNever()
                .HasColumnName("ItineraryID");
            entity.Property(e => e.ArrivalFlight).HasMaxLength(200);
            entity.Property(e => e.DepartureFlight).HasMaxLength(200);
            entity.Property(e => e.DisplayName).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Enteredby).HasMaxLength(100);
            entity.Property(e => e.InclusionNotes).IsUnicode(false);
            entity.Property(e => e.ItineraryFolderPath).HasMaxLength(500);
            entity.Property(e => e.ItineraryName).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(25);
        });

        modelBuilder.Entity<ItineraryFolderinfo>(entity =>
        {
            entity.HasKey(e => e.Folderid).HasName("PK__Itinerar__ACC81C57DF813F40");

            entity.ToTable("ItineraryFolderinfo");

            entity.Property(e => e.FolderFullpath).IsUnicode(false);
            entity.Property(e => e.Parentfolder)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MarginSetting>(entity =>
        {
            entity.HasKey(e => e.SettingsId).HasName("PK__MarginSe__991B19DC6301B8AE");

            entity.ToTable("MarginSetting");

            entity.HasIndex(e => e.Overrideall, "Indx_Overrideall");

            entity.Property(e => e.SettingsId)
                .ValueGeneratedNever()
                .HasColumnName("SettingsID");
            entity.Property(e => e.Overrideall).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<OptionTypesforRoom>(entity =>
        {
            entity.HasKey(e => e.OptionTypeRoomid).HasName("PK__OptionTy__E7036281E16B77D8");

            entity.ToTable("OptionTypesforRoom");

            entity.HasIndex(e => e.OptionTypesName, "Indx_OptionTypesName");

            entity.Property(e => e.OptionTypeRoomid).ValueGeneratedNever();
            entity.Property(e => e.OptionTypesName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PassengerDetail>(entity =>
        {
            entity.HasKey(e => e.Passengerid).HasName("PK__Passenge__88905388DEF46873");

            entity.HasIndex(e => e.DisplayName, "Indx_DisplayName");

            entity.HasIndex(e => e.Email, "Indx_Email");

            entity.Property(e => e.Passengerid).ValueGeneratedNever();
            entity.Property(e => e.AgentNet).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CmmOvrd).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyName).IsUnicode(false);
            entity.Property(e => e.DefaultPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.LastName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PriceOverride).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Room)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PassengerGroup>(entity =>
        {
            entity.HasKey(e => e.Passengergroupid).HasName("PK__Passenge__F368977FB53F391A");

            entity.ToTable("PassengerGroup");

            entity.HasIndex(e => e.Passengergroupname, "Indx_Passengergroup");

            entity.Property(e => e.Passengergroupid).ValueGeneratedNever();
            entity.Property(e => e.Passengergroupname)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PassengerType>(entity =>
        {
            entity.HasKey(e => e.PassengerTypeid).HasName("PK__Passenge__EE60AEEE8C5426C7");

            entity.ToTable("PassengerType");

            entity.HasIndex(e => e.PassengerTypename, "Indx_PassengerTypename");

            entity.Property(e => e.PassengerTypeid).ValueGeneratedNever();
            entity.Property(e => e.PassengerTypename)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paxinformation>(entity =>
        {
            entity.HasKey(e => e.Paxid).HasName("PK__Paxinfor__F7794A9905994E49");

            entity.ToTable("Paxinformation");

            entity.HasIndex(e => e.PaxNumbers, "Indx_Paxnumbers");

            entity.Property(e => e.Paxid).ValueGeneratedNever();
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__PaymentD__9B556A58B1C7ECC8");

            entity.HasIndex(e => e.DateofPayment, "Indx_DateofPayment");

            entity.HasIndex(e => e.Personname, "Indx_Personname");

            entity.Property(e => e.PaymentId)
                .ValueGeneratedNever()
                .HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Details)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FeePercent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");
            entity.Property(e => e.Personname)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RefundPaymentTotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sale).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Paymenttype>(entity =>
        {
            entity.HasKey(e => e.Paymenttypesid).HasName("PK__Paymentt__13680D200F7A6391");

            entity.HasIndex(e => e.Paymenttypesname, "Indx_Paymenttypesname");

            entity.Property(e => e.Paymenttypesid).ValueGeneratedNever();
            entity.Property(e => e.Isdefault).HasDefaultValueSql("((0))");
            entity.Property(e => e.Paymenttypesname)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PickupDropLocation>(entity =>
        {
            entity.HasKey(e => e.PickupDropLocationId).HasName("PK__PickupDr__0CA25ACCBA8767CA");

            entity.ToTable("PickupDropLocation");

            entity.HasIndex(e => e.LocationName, "Indx_LocationName");

            entity.Property(e => e.PickupDropLocationId).ValueGeneratedNever();
            entity.Property(e => e.LocationName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("PK__Region__ACD844A3C20F76A2");

            entity.ToTable("Region");

            entity.HasIndex(e => e.StatesId, "Indx_StatesId");

            entity.Property(e => e.RegionId).ValueGeneratedNever();
            entity.Property(e => e.RegionName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.HasKey(e => e.RequestStatusId).HasName("PK__RequestS__7094B79B7D699808");

            entity.ToTable("RequestStatus");

            entity.Property(e => e.RequestStatusId).ValueGeneratedNever();
            entity.Property(e => e.RequestStatusName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Roomtypesid).HasName("PK__RoomType__6AC30E663F8DFE33");

            entity.HasIndex(e => e.OptionTypeRoomid, "Indx_OptionTypeRoomid");

            entity.Property(e => e.Roomtypesid).ValueGeneratedNever();
            entity.Property(e => e.SellPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("PK__ServiceT__8ADFAA0C8C955402");

            entity.ToTable("ServiceType");

            entity.Property(e => e.ServiceTypeId)
                .ValueGeneratedNever()
                .HasColumnName("ServiceTypeID");
            entity.Property(e => e.IsDefault).HasDefaultValueSql("((0))");
            entity.Property(e => e.ServiceTypeName).HasMaxLength(250);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.SettingsId).HasName("PK__Settings__2DA7B9D2D012B9BC");

            entity.Property(e => e.SettingsId).HasColumnName("settingsID");
            entity.Property(e => e.FieldName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FieldValue)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StatesId).HasName("PK__States__AC838DA8FBDDC874");

            entity.HasIndex(e => e.CountryId, "Indx_CountryId");

            entity.Property(e => e.StatesId).ValueGeneratedNever();
            entity.Property(e => e.StatesName)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SupplierAutoNumber>(entity =>
        {
            entity.HasKey(e => e.Pkid).HasName("PK__Supplier__A7DF3BD0734F51F8");

            entity.ToTable("SupplierAutoNumber");
        });

        modelBuilder.Entity<SupplierCommunication>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__Supplier__2907A87EAC8DDC9A");

            entity.ToTable("SupplierCommunication");

            entity.HasIndex(e => e.ContentName, "Indx_ContentName");

            entity.HasIndex(e => e.Heading, "Indx_Heading");

            entity.HasIndex(e => e.SupplierId, "Indx_SupplierId");

            entity.Property(e => e.ContentId)
                .ValueGeneratedNever()
                .HasColumnName("ContentID");
            entity.Property(e => e.ContentFor).HasMaxLength(250);
            entity.Property(e => e.ContentName).HasMaxLength(500);
            entity.Property(e => e.Heading).HasMaxLength(250);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
        });

        modelBuilder.Entity<SupplierEmailSetting>(entity =>
        {
            entity.HasKey(e => e.SupplierEmailSettingsid).HasName("PK__Supplier__4C1362B297FDFD29");

            entity.HasIndex(e => e.FromAddress, "Indx_FromAddEmail");

            entity.HasIndex(e => new { e.FromAddress, e.EmailSubject }, "Indx_FromAddEmailSub");

            entity.Property(e => e.Attachment).HasMaxLength(2000);
            entity.Property(e => e.Bcc).HasMaxLength(1000);
            entity.Property(e => e.EmailBodyContentTemplate).HasMaxLength(2000);
            entity.Property(e => e.EmailSubject).HasMaxLength(2000);
            entity.Property(e => e.EmailTemplate).HasMaxLength(2000);
            entity.Property(e => e.Error).HasMaxLength(2000);
            entity.Property(e => e.FromAddress).HasMaxLength(1000);
            entity.Property(e => e.ToAddress).HasMaxLength(1000);
        });

        modelBuilder.Entity<SupplierInformation>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4F5E8F0ED");

            entity.ToTable("SupplierInformation");

            entity.HasIndex(e => new { e.SupplierName, e.SupplierAutoId }, "Indx_SuppName_AutoID");

            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.CustomCode).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Fax).HasMaxLength(30);
            entity.Property(e => e.FreePh).HasMaxLength(20);
            entity.Property(e => e.Hosts).HasMaxLength(250);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PostalAddress).HasMaxLength(1000);
            entity.Property(e => e.Postcode).HasMaxLength(20);
            entity.Property(e => e.Street).HasMaxLength(250);
            entity.Property(e => e.SupplierComments).HasMaxLength(2000);
            entity.Property(e => e.SupplierDescription).HasMaxLength(2000);
            entity.Property(e => e.SupplierFolderPath).HasMaxLength(2000);
            entity.Property(e => e.SupplierName).HasMaxLength(250);
            entity.Property(e => e.Website).HasMaxLength(150);
        });

        modelBuilder.Entity<SupplierMarginSetting>(entity =>
        {
            entity.HasKey(e => e.SupMarginSettingid).HasName("PK__Supplier__C3F27B8AEA5445BC");

            entity.Property(e => e.SupMarginSettingid).ValueGeneratedNever();
            entity.Property(e => e.MarginValue).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<SupplierPaymentsRecord>(entity =>
        {
            entity.HasKey(e => e.SupplierPaymentId).HasName("PK__Supplier__DC6B0E3ED505DC90");

            entity.HasIndex(e => e.BookingId, "Indx_BookingId");

            entity.HasIndex(e => e.InvoiceDate, "Indx_InvoiceDate");

            entity.HasIndex(e => e.InvoiceId, "Indx_InvoiceId");

            entity.HasIndex(e => e.ItineraryId, "Indx_ItineraryId");

            entity.HasIndex(e => e.SupplierId, "Indx_SupplierId");

            entity.Property(e => e.SupplierPaymentId).ValueGeneratedNever();
            entity.Property(e => e.BookingIdidentifier).HasColumnName("BookingIDIdentifier");
            entity.Property(e => e.ConvertedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyExchangeRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Notes).IsUnicode(false);
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TotalOutstanding).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<SupplierService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Supplier__C51BB00A2D9FB62C");

            entity.ToTable("SupplierService");

            entity.HasIndex(e => e.SupplierId, "Indx_SupplierId");

            entity.Property(e => e.ServiceId).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.ServiceName).HasMaxLength(250);
        });

        modelBuilder.Entity<SupplierServiceDetailsRate>(entity =>
        {
            entity.HasKey(e => e.SupplierServiceDetailsRateId).HasName("PK__Supplier__6A9E3AB2479971B6");

            entity.ToTable("SupplierServiceDetailsRate");

            entity.HasIndex(e => e.SupplierServiceId, "Indx_SupplServiceIdForRate");

            entity.Property(e => e.SupplierServiceDetailsRateId).ValueGeneratedNever();
        });

        modelBuilder.Entity<SupplierServiceDetailsWarning>(entity =>
        {
            entity.HasKey(e => e.SupplierServiceDetailsWarningId).HasName("PK__Supplier__9C18581CC893874C");

            entity.ToTable("SupplierServiceDetailsWarning");

            entity.HasIndex(e => e.SupplierServiceId, "Indx_SupplServiceIdForWarn");

            entity.Property(e => e.SupplierServiceDetailsWarningId)
                .ValueGeneratedNever()
                .HasColumnName("SupplierServiceDetailsWarningID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Messagefor).HasMaxLength(100);
        });

        modelBuilder.Entity<SupplierServicePriceEditRate>(entity =>
        {
            entity.HasKey(e => e.PriceEditRateId).HasName("PK__Supplier__0710D9C228CAC62E");

            entity.ToTable("SupplierServicePriceEditRate");

            entity.HasIndex(e => e.PricingOptionId, "Indx_PricingOptionId");

            entity.HasIndex(e => e.SupplierServiceId, "Indx_SupplServiceIdForPriceEdit");

            entity.Property(e => e.PriceEditRateId).ValueGeneratedNever();
            entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.GrossPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MarkupPercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<SupplierServicePricingoption>(entity =>
        {
            entity.HasKey(e => e.PricingOptionId).HasName("PK__Supplier__8817FCAEC3CB3E4D");

            entity.ToTable("SupplierServicePricingoption");

            entity.HasIndex(e => e.SupplierServiceId, "Indx_SupplServiceIdforPrice");

            entity.HasIndex(e => e.SupplierServiceDetailsRateId, "Indx_SupplierServiceDetailsRateId");

            entity.Property(e => e.PricingOptionId).ValueGeneratedNever();
            entity.Property(e => e.CommissionPercentage).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.GrossPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.MarkupPercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PricingOptionName).HasMaxLength(250);
        });

        modelBuilder.Entity<Supplierfolderinfo>(entity =>
        {
            entity.HasKey(e => e.Supplierfolderinfoid).HasName("PK__supplier__7E22288B31B51A56");

            entity.ToTable("supplierfolderinfo");

            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createdon).HasColumnName("createdon");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Supplierfolderpath).HasMaxLength(2000);
        });

        modelBuilder.Entity<TestInsert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestInse__3214EC07C9A84708");

            entity.ToTable("TestInsert");

            entity.Property(e => e.TestEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TestName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.UserRoles, "Indx_UserDetUserRoles");

            entity.HasIndex(e => e.UserName, "Indx_UserName");

            entity.HasIndex(e => new { e.UserId, e.UserName }, "idx_useridname");

            entity.Property(e => e.ConfirmPassword).HasMaxLength(100);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.EmailSignature).HasMaxLength(500);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.UserRole1, "Indx_UserRoles");

            entity.Property(e => e.UserRoldId).HasColumnName("UserRoldID");
            entity.Property(e => e.UserRole1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UserRole");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
