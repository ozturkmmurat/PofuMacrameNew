using System;

namespace Business.Constans.Html.Mail
{
    public class MailHtml
    {
        private const string Head = @"<!DOCTYPE html>
        <html>
        <head>
          <meta charset=""utf-8"">
          <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
          <title>PofuMacrame</title>
          <style>
            body { margin: 0; padding: 20px; background: #f0f0f0; font-family: sans-serif; font-size: 16px; line-height: 1.5; color: #333; }
            .box { max-width: 560px; margin: 0 auto; background: #fff; border-radius: 8px; overflow: hidden; }
            .cell { padding: 16px 24px; }
            .title { font-size: 18px; font-weight: 600; margin-bottom: 8px; }
            .row { padding: 12px 24px; border-top: 1px solid #eee; }
            .label { font-weight: 600; color: #333; }
            .btn { display: inline-block; padding: 12px 24px; background: #E9703E; color: #fff !important; text-decoration: none; border-radius: 4px; margin-top: 16px; }
          </style>
        </head>
        <body>
          <div class=""box"">";

                private const string Foot = @"
          </div>
        </body>
        </html>";

        private static string Wrap(string body) => Head + body + Foot;

        private static string Row(string label, string value) =>
            $@"    <div class=""row""><span class=""label"">{label}</span><br>{value}</div>";

        public static string Register(string firstName, string lastName, string email)
        {
            var body = @"
            <div class=""cell"">
              <div class=""title"">Web sitenize yeni bir üye kaydolmuştur</div>
            </div>"
                + Row("Ad Soyad", $"{firstName} {lastName}")
                + Row("Email", email);
            return Wrap(body);
        }

        public static string CreateOrder(string orderCode)
        {
            var body = @"
            <div class=""cell"">
              <div class=""title"">Siparişiniz başarıyla alınmıştır. Alışverişiniz için teşekkür ederiz.</div>
              <a href=""#"" class=""btn"">Siparişlerim</a>
            </div>";
            return Wrap(body);
        }

        public static string CancelOrder(string firstName, string lastName, string email, string orderCode)
        {
            var body = @"
            <div class=""cell"">
              <div class=""title"">Sipariş İptal Edildi</div>
            </div>"
                + Row("Sipariş No", orderCode)
                + Row("Ad Soyad", $"{firstName} {lastName}")
                + Row("Email", email);
            return Wrap(body);
        }

        public static string RefundingProduct(string firstName, string lastName, string email, int variantId)
        {
            var body = @"
            <div class=""cell"">
              <div class=""title"">Ürün iade talebiniz alındı</div>
            </div>";
                //+ Row("Ürün No", variantId.ToString())
                //+ Row("Ad Soyad", $"{firstName} {lastName}")
                //+ Row("Email", email);
            return Wrap(body);
        }

        public static string OrderShipped(string orderCode)
        {
            var body = @"
            <div class=""cell"">
              <div class=""title"">Siparişiniz Kargoya Verildi. Bizi tercih ettiğiniz için teşekkür ederiz.</div>
            </div>"
                + Row("Sipariş Kodu", orderCode);
                //+ Row("Takip Kodu", code);
            return Wrap(body);
        }

        public static string AdminCancelOrder()
        {
            var body = @"
        <div class=""cell"">
          <div class=""title"">Siparişiniz iptal edilmiştir.</div>
        </div>";
            return Wrap(body);
        }

        public static string AdminRefundingProduct()
        {
            var body = @"
            <div class=""cell"">
              <div class=""title"">Ürün siparişiniz iptal edilmiştir.</div>
            </div>";
            return Wrap(body);
        }

        public static string ForgotPasswordCode(string code)
        {
            var body = $@"
            <div class=""cell"">
              <div class=""title"">Şifre Sıfırlama Kodu</div>
              <p>{code}</p>
            </div>";
            return Wrap(body);
        }

        public static string ResetPasswordLink(string url)
        {
            var body = $@"
            <div class=""cell"">
              <div class=""title"">Şifrenizi aşağıdaki buton ile yenileyebilirsiniz.</div>
              <a href=""{url}"" class=""btn"">Şifremi Yenile</a>
            </div>";
            return Wrap(body);
        }
    }
}
