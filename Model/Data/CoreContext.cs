using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model.Data
{
    public partial class CoreContext : DbContext
    {
        public CoreContext()
        {
        }

        public CoreContext(DbContextOptions<CoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityInfo> ActivityInfo { get; set; }
        public virtual DbSet<ActivityLog> ActivityLog { get; set; }
        public virtual DbSet<AddDrawCountLog> AddDrawCountLog { get; set; }
        public virtual DbSet<BookConfig> BookConfig { get; set; }
        public virtual DbSet<BookInfo> BookInfo { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<MemberDrawCount> MemberDrawCount { get; set; }
        public virtual DbSet<MemberInfo> MemberInfo { get; set; }
        public virtual DbSet<MemberOuathCode> MemberOuathCode { get; set; }
        public virtual DbSet<OrderPromotion> OrderPromotion { get; set; }
        public virtual DbSet<PaymentConfig> PaymentConfig { get; set; }
        public virtual DbSet<PaymentLog> PaymentLog { get; set; }
        public virtual DbSet<PayOrder> PayOrder { get; set; }
        public virtual DbSet<PinConfig> PinConfig { get; set; }
        public virtual DbSet<PinInfo> PinInfo { get; set; }
        public virtual DbSet<PinOrder> PinOrder { get; set; }
        public virtual DbSet<PrizeInfo> PrizeInfo { get; set; }
        public virtual DbSet<ProductInfo> ProductInfo { get; set; }
        public virtual DbSet<PromotionConfig> PromotionConfig { get; set; }
        public virtual DbSet<ShopProductInfo> ShopProductInfo { get; set; }
        public virtual DbSet<WxConfig> WxConfig { get; set; }

        public static string ConnnectString { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConnnectString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityInfo>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("activity_info");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .HasColumnName("CONTENT")
                    .HasColumnType("longtext");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("CREATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeptCode)
                    .HasColumnName("DEPT_CODE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.EndTime)
                    .HasColumnName("END_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.JoinConfig)
                    .HasColumnName("JOIN_CONFIG")
                    .HasColumnType("longtext");

                entity.Property(e => e.Kind)
                    .HasColumnName("KIND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OwnerAccount)
                    .HasColumnName("OWNER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ProductConfig)
                    .HasColumnName("PRODUCT_CONFIG")
                    .HasColumnType("longtext");

                entity.Property(e => e.StartTime)
                    .HasColumnName("START_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ThumbnailUrl)
                    .HasColumnName("THUMBNAIL_URL")
                    .HasColumnType("longtext");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("activity_log");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActivityId)
                    .HasColumnName("ACTIVITY_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("CREATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OwnerAccount)
                    .HasColumnName("OWNER_ACCOUNT")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<AddDrawCountLog>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("add_draw_count_log");

                entity.HasIndex(e => new { e.MemberAccount, e.Kind })
                    .HasName("MEMBER_ACCOUNT");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActId)
                    .HasColumnName("ACT_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Counter)
                    .HasColumnName("COUNTER")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("CREATE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kind)
                    .HasColumnName("KIND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<BookConfig>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("book_config");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Config)
                    .HasColumnName("CONFIG")
                    .HasColumnType("longtext");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("CREATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.EndTime)
                    .HasColumnName("END_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.StartTime)
                    .HasColumnName("START_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<BookInfo>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("book_info");

                entity.HasIndex(e => e.MemberAccount)
                    .HasName("MEMBER_ACCOUNT");

                entity.HasIndex(e => e.OrderNo)
                    .HasName("ORDER_NO")
                    .IsUnique();

                entity.HasIndex(e => e.ProductNo)
                    .HasName("PRODUCT_NO");

                entity.HasIndex(e => e.ProductSkuNo)
                    .HasName("PRODUCT_SKU_NO");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("ADDRESS")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.AddressId)
                    .HasColumnName("ADDRESS_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Area)
                    .HasColumnName("AREA")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ConfigId)
                    .HasColumnName("CONFIG_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Counter)
                    .HasColumnName("COUNTER")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CouponId)
                    .HasColumnName("COUPON_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CouponTicket)
                    .HasColumnName("COUPON_TICKET")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.EndTime)
                    .HasColumnName("END_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OrderNo)
                    .HasColumnName("ORDER_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ProductConfig)
                    .HasColumnName("PRODUCT_CONFIG")
                    .HasColumnType("longtext");

                entity.Property(e => e.ProductNo)
                    .HasColumnName("PRODUCT_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ProductSkuNo)
                    .HasColumnName("PRODUCT_SKU_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Province)
                    .HasColumnName("PROVINCE")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ShopOrderNo)
                    .HasColumnName("SHOP_ORDER_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.StartTime)
                    .HasColumnName("START_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("logs");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Exception).HasColumnType("text");

                entity.Property(e => e.Level).HasColumnType("varchar(15)");

                entity.Property(e => e.Logger).HasColumnType("varchar(20)");

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.ModuleName).HasColumnType("varchar(20)");

                entity.Property(e => e.Properties).HasColumnType("text");

                entity.Property(e => e.Timestamp).HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<MemberDrawCount>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("member_draw_count");

                entity.HasIndex(e => e.MemberAccount)
                    .HasName("MEMBER_ACCOUNT")
                    .IsUnique();

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActId)
                    .HasColumnName("ACT_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Counter)
                    .HasColumnName("COUNTER")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CurrentCount)
                    .HasColumnName("CURRENT_COUNT")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<MemberInfo>(entity =>
            {
                entity.ToTable("member_info");

                entity.HasIndex(e => e.AccountId)
                    .HasName("accountid")
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .HasName("email")
                    .IsUnique();

                entity.HasIndex(e => e.LastLog)
                    .HasName("LAST_LOG");

                entity.HasIndex(e => e.LoginId)
                    .HasName("login_id")
                    .IsUnique();

                entity.HasIndex(e => e.OldWxOpenId)
                    .HasName("wxopenid")
                    .IsUnique();

                entity.HasIndex(e => e.Phone)
                    .HasName("phone")
                    .IsUnique();

                entity.HasIndex(e => e.WxOpenId)
                    .HasName("WX_OPEN_ID");

                entity.HasIndex(e => e.WxUnionid)
                    .HasName("WX_UNIONID")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccountId)
                    .HasColumnName("ACCOUNT_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.AddDate)
                    .HasColumnName("ADD_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Address)
                    .HasColumnName("ADDRESS")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.AdvancePayment).HasColumnName("ADVANCE_PAYMENT");

                entity.Property(e => e.Answer)
                    .HasColumnName("ANSWER")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Birthday)
                    .HasColumnName("BIRTHDAY")
                    .HasColumnType("datetime");

                entity.Property(e => e.CardTag)
                    .HasColumnName("CARD_TAG")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Cashbox)
                    .HasColumnName("CASHBOX")
                    .HasColumnType("decimal(10,0)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Coins)
                    .HasColumnName("COINS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Country)
                    .HasColumnName("COUNTRY")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.DeptCode)
                    .HasColumnName("DEPT_CODE")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Icq)
                    .HasColumnName("ICQ")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IdentityNo)
                    .HasColumnName("IDENTITY_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IdentityType)
                    .HasColumnName("IDENTITY_TYPE")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Introducer)
                    .HasColumnName("INTRODUCER")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Introduction)
                    .HasColumnName("INTRODUCTION")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Language)
                    .HasColumnName("LANGUAGE")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.LastLog)
                    .HasColumnName("LAST_LOG")
                    .HasColumnType("datetime");

                entity.Property(e => e.Level)
                    .HasColumnName("LEVEL")
                    .HasColumnType("int(5)");

                entity.Property(e => e.LogTimes)
                    .HasColumnName("LOG_TIMES")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LoginId)
                    .HasColumnName("LOGIN_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MemberName)
                    .HasColumnName("MEMBER_NAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Msn)
                    .HasColumnName("MSN")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.NickName)
                    .HasColumnName("NICK_NAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OldWxOpenId)
                    .HasColumnName("OLD_WX_OPEN_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OwnerAccount)
                    .HasColumnName("OWNER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Password)
                    .HasColumnName("PASSWORD")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PhotoUrl)
                    .HasColumnName("PHOTO_URL")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("POSTAL_CODE")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Province)
                    .HasColumnName("PROVINCE")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Qq)
                    .HasColumnName("QQ")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.QqOpenId)
                    .HasColumnName("QQ_OPEN_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Question)
                    .HasColumnName("QUESTION")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Score)
                    .HasColumnName("SCORE")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SellPassword)
                    .HasColumnName("SELL_PASSWORD")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Sex)
                    .HasColumnName("SEX")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.SinaOpenId)
                    .HasColumnName("SINA_OPEN_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.WxOpenId)
                    .HasColumnName("WX_OPEN_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.WxUnionid)
                    .HasColumnName("WX_UNIONID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.XcxOpenId)
                    .HasColumnName("XCX_OPEN_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ZlOpenId)
                    .HasColumnName("ZL_OPEN_ID")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<MemberOuathCode>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("member_ouath_code");

                entity.HasIndex(e => e.MemberAccount)
                    .HasName("MEMBER_ACCOUNT");

                entity.HasIndex(e => e.OuathCode)
                    .HasName("OUATH_CODE");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("CREATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OuathCode)
                    .HasColumnName("OUATH_CODE")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<OrderPromotion>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("order_promotion");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Config)
                    .HasColumnName("CONFIG")
                    .HasColumnType("text");

                entity.Property(e => e.DiscountFee)
                    .HasColumnName("DISCOUNT_FEE")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OrderNo)
                    .HasColumnName("ORDER_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.RecordNum)
                    .HasColumnName("RECORD_NUM")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RecordTime)
                    .HasColumnName("RECORD_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Reulst)
                    .HasColumnName("REULST")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RuleId)
                    .HasColumnName("RULE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<PaymentConfig>(entity =>
            {
                entity.ToTable("payment_config");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AppId)
                    .HasColumnName("APP_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CreateRefund)
                    .HasColumnName("CREATE_REFUND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.InterfaceId)
                    .HasColumnName("INTERFACE_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Kind)
                    .HasColumnName("KIND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OwnerAccount)
                    .HasColumnName("OWNER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PayType)
                    .HasColumnName("PAY_TYPE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PayUrl)
                    .HasColumnName("PAY_URL")
                    .HasColumnType("varchar(1024)");

                entity.Property(e => e.PaymentName)
                    .HasColumnName("PAYMENT_NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PrivateKey)
                    .HasColumnName("PRIVATE_KEY")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.QueryOrder)
                    .HasColumnName("QUERY_ORDER")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.QueryRefund)
                    .HasColumnName("QUERY_REFUND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SellerEmail)
                    .HasColumnName("SELLER_EMAIL")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Sort)
                    .HasColumnName("SORT")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.SupportCoins)
                    .HasColumnName("SUPPORT_COINS")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.SynchroCommand)
                    .HasColumnName("Synchro_Command")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PaymentLog>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("payment_log");

                entity.HasIndex(e => e.PaymentAccount)
                    .HasName("ORDER_NO");

                entity.HasIndex(e => e.PaymentNo)
                    .HasName("PAYMENT_NO");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("CREATE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.PayStatus)
                    .HasColumnName("PAY_STATUS")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.PayTime)
                    .HasColumnName("PAY_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.PaymentAccount)
                    .HasColumnName("PAYMENT_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PaymentFee)
                    .HasColumnName("PAYMENT_FEE")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("PAYMENT_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PaymentNo)
                    .HasColumnName("PAYMENT_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TradeNo)
                    .HasColumnName("TRADE_NO")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PayOrder>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("pay_order");

                entity.HasIndex(e => e.OrderNo)
                    .HasName("ORDER_NO");

                entity.HasIndex(e => new { e.OrderNo, e.MemberAccount })
                    .HasName("ORDER_NO, MEMBER_ACCOUNT");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("CREATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.DiscountFee)
                    .HasColumnName("DISCOUNT_FEE")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Kind)
                    .HasColumnName("KIND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OrderNo)
                    .HasColumnName("ORDER_NO")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PayFee)
                    .HasColumnName("PAY_FEE")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.PayTime)
                    .HasColumnName("PAY_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("PAYMENT_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductFee)
                    .HasColumnName("PRODUCT_FEE")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.ShippingFee)
                    .HasColumnName("SHIPPING_FEE")
                    .HasColumnType("decimal(11,2)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TotalFee)
                    .HasColumnName("TOTAL_FEE")
                    .HasColumnType("decimal(11,2)");
            });

            modelBuilder.Entity<PinConfig>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("pin_config");

                entity.HasIndex(e => e.PingId)
                    .HasName("PING_ID")
                    .IsUnique();

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Config)
                    .HasColumnName("CONFIG")
                    .HasColumnType("longtext");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.MaxDate)
                    .HasColumnName("MAX_DATE")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MinCount)
                    .HasColumnName("MIN_COUNT")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PingId)
                    .HasColumnName("PING_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PinInfo>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("pin_info");

                entity.HasIndex(e => e.MemberAccount)
                    .HasName("MEMBER_ACCOUNT");

                entity.HasIndex(e => e.PingId)
                    .HasName("PING_ID");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Config)
                    .HasColumnName("CONFIG")
                    .HasColumnType("longtext");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("CREATE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.MaxDate)
                    .HasColumnName("MAX_DATE")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MinCount)
                    .HasColumnName("MIN_COUNT")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PingId)
                    .HasColumnName("PING_ID")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PinOrder>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("pin_order");

                entity.HasIndex(e => e.MainId)
                    .HasName("MAIN_ID");

                entity.HasIndex(e => e.MemberAccount)
                    .HasName("MEMBER_ACCOUNT");

                entity.HasIndex(e => e.OrderNo)
                    .HasName("ORDER_NO");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("CREATE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.MainId)
                    .HasColumnName("MAIN_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OrderNo)
                    .HasColumnName("ORDER_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ProductConfig)
                    .HasColumnName("PRODUCT_CONFIG")
                    .HasColumnType("longtext");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<PrizeInfo>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("prize_info");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActId)
                    .HasColumnName("ACT_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CouponId)
                    .HasColumnName("COUPON_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("CREATE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kind)
                    .HasColumnName("KIND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MemberAccount)
                    .HasColumnName("MEMBER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PrizeCode)
                    .HasColumnName("PRIZE_CODE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PrizeName)
                    .HasColumnName("PRIZE_NAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UseDate)
                    .HasColumnName("USE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.WinnerInfo)
                    .HasColumnName("WINNER_INFO")
                    .HasColumnType("varchar(2000)");
            });

            modelBuilder.Entity<ProductInfo>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("product_info");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasColumnType("longtext");

                entity.Property(e => e.ProductDesc)
                    .HasColumnName("PRODUCT_DESC")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ProductName)
                    .HasColumnName("PRODUCT_NAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ProductNo)
                    .HasColumnName("PRODUCT_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SaleCount)
                    .HasColumnName("SALE_COUNT")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SalePrice)
                    .HasColumnName("SALE_PRICE")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Sort)
                    .HasColumnName("SORT")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'99'");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TypePrefix)
                    .HasColumnName("TYPE_PREFIX")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PromotionConfig>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("promotion_config");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Config)
                    .HasColumnName("CONFIG")
                    .HasColumnType("text");

                entity.Property(e => e.EndTime)
                    .HasColumnName("END_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Memo)
                    .HasColumnName("MEMO")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Priority)
                    .HasColumnName("PRIORITY")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Result)
                    .HasColumnName("RESULT")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.StartTime)
                    .HasColumnName("START_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<ShopProductInfo>(entity =>
            {
                entity.ToTable("shop_product_info");

                entity.HasIndex(e => e.ProductNo)
                    .HasName("ProductNo")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime)
                    .HasColumnName("ADD_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.AvailableCoupon)
                    .HasColumnName("AVAILABLE_COUPON")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Brand)
                    .HasColumnName("BRAND")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ChatRoomLink)
                    .HasColumnName("CHAT_ROOM_LINK")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Coins)
                    .HasColumnName("COINS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CustomField)
                    .HasColumnName("CUSTOM_FIELD")
                    .HasColumnType("text");

                entity.Property(e => e.DeptCode)
                    .HasColumnName("DEPT_CODE")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("longtext");

                entity.Property(e => e.EndTime)
                    .HasColumnName("END_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExchangeTimes)
                    .HasColumnName("EXCHANGE_TIMES")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'5'");

                entity.Property(e => e.GiftContent)
                    .HasColumnName("GIFT_CONTENT")
                    .HasColumnType("longtext");

                entity.Property(e => e.GroundingTime)
                    .HasColumnName("GROUNDING_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hits)
                    .HasColumnName("HITS")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InteriorCode)
                    .HasColumnName("INTERIOR_CODE")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IsCommend)
                    .HasColumnName("IS_COMMEND")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.IsDel)
                    .HasColumnName("IS_DEL")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsForSale)
                    .HasColumnName("IS_FOR_SALE")
                    .HasColumnType("varchar(1023)");

                entity.Property(e => e.IsHot)
                    .HasColumnName("IS_HOT")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.IsLimitTime)
                    .HasColumnName("IS_LIMIT_TIME")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.IsSpecialOffer)
                    .HasColumnName("IS_SPECIAL_OFFER")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IsVirtual)
                    .HasColumnName("IS_VIRTUAL")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Keyword)
                    .HasColumnName("KEYWORD")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Kind)
                    .HasColumnName("KIND")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.KindRuleId)
                    .HasColumnName("KIND_RULE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Level)
                    .HasColumnName("LEVEL")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.MarketPrice)
                    .HasColumnName("MARKET_PRICE")
                    .HasColumnType("decimal(19,2)");

                entity.Property(e => e.MiniNum)
                    .HasColumnName("MINI_NUM")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OwnerAccount)
                    .HasColumnName("OWNER_ACCOUNT")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PackContent)
                    .HasColumnName("PACK_CONTENT")
                    .HasColumnType("longtext");

                entity.Property(e => e.PackageProducts)
                    .HasColumnName("PACKAGE_PRODUCTS")
                    .HasColumnType("varchar(2000)");

                entity.Property(e => e.ParamContent)
                    .HasColumnName("PARAM_CONTENT")
                    .HasColumnType("longtext");

                entity.Property(e => e.ProductImg)
                    .HasColumnName("PRODUCT_IMG")
                    .HasColumnType("longtext");

                entity.Property(e => e.ProductName)
                    .HasColumnName("PRODUCT_NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ProductNameTag)
                    .HasColumnName("PRODUCT_NAME_TAG")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ProductNo)
                    .HasColumnName("PRODUCT_NO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.QrcodeUrl)
                    .HasColumnName("QRCODE_URL")
                    .HasColumnType("longtext");

                entity.Property(e => e.RemarkConfig)
                    .HasColumnName("REMARK_CONFIG")
                    .HasColumnType("varchar(2047)");

                entity.Property(e => e.SalePrice)
                    .HasColumnName("SALE_PRICE")
                    .HasColumnType("decimal(19,2)");

                entity.Property(e => e.Score)
                    .HasColumnName("SCORE")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SellCount)
                    .HasColumnName("SELL_COUNT")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ShippingTemplate)
                    .HasColumnName("SHIPPING_TEMPLATE")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.ShortDesc)
                    .HasColumnName("SHORT_DESC")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ShortDescTag)
                    .HasColumnName("SHORT_DESC_TAG")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Sort)
                    .HasColumnName("SORT")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartTime)
                    .HasColumnName("START_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Storage)
                    .HasColumnName("STORAGE")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StoreId)
                    .HasColumnName("STORE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SupportNopay)
                    .HasColumnName("SUPPORT_NOPAY")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.TagIds)
                    .HasColumnName("TAG_IDS")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ThumbnailImg)
                    .HasColumnName("THUMBNAIL_IMG")
                    .HasColumnType("longtext");

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TypePrefix)
                    .HasColumnName("TYPE_PREFIX")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Video)
                    .HasColumnName("VIDEO")
                    .HasColumnType("longtext");

                entity.Property(e => e.Weight)
                    .HasColumnName("WEIGHT")
                    .HasColumnType("decimal(19,2)");
            });

            modelBuilder.Entity<WxConfig>(entity =>
            {
                entity.HasKey(e => e.Recid);

                entity.ToTable("wx_config");

                entity.Property(e => e.Recid)
                    .HasColumnName("RECID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccessTokenUpdateTime)
                    .HasColumnName("ACCESS_TOKEN_UPDATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.AccessTokenValue)
                    .HasColumnName("ACCESS_TOKEN_VALUE")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.AppId)
                    .HasColumnName("APP_ID")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.AppSecret)
                    .HasColumnName("APP_SECRET")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.InterfaceToken)
                    .HasColumnName("INTERFACE_TOKEN")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.InterfaceUrl)
                    .HasColumnName("INTERFACE_URL")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.JsTicketUpdateTime)
                    .HasColumnName("JS_TICKET_UPDATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.JsTicketValue)
                    .HasColumnName("JS_TICKET_VALUE")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.LogoUrl)
                    .HasColumnName("LOGO_URL")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OauthEnable)
                    .HasColumnName("OAUTH_ENABLE")
                    .HasColumnType("varchar(5)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.OwnerAccount)
                    .HasColumnName("OWNER_ACCOUNT")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ServerDomain)
                    .HasColumnName("SERVER_DOMAIN")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.WebsiteName)
                    .HasColumnName("WEBSITE_NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.WxAccount)
                    .HasColumnName("WX_ACCOUNT")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.WxAccountKind)
                    .HasColumnName("WX_ACCOUNT_KIND")
                    .HasColumnType("varchar(50)");
            });
        }
    }
}
