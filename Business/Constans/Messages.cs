using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constans
{
    public class Messages
    {
        public static string SuccessGet = "Verilerin hepsi başarıyla listelendi";
        public static string UnSuccessGet = "Veriler listelenemedi";
        public static string SuccessAdd = "Veri başarıyla eklendi.";
        public static string UnSuccessAdd = "Veri eklenirken bir sorun oluştu.";
        public static string SuccessUpdate = "Veri başarıyla güncellendi.";
        public static string UnSuccessUpdate = "Veri güncellenirken bir sorun oluştu.";
        public static string SuccessDelete = "Veri başarıyla silindi";
        public static string UnSuccessDelete = "Veri silinirken bir sorun oluştu.";
        public static string ImageLimit = "Bir varyantın en fazla 5 fotoğrafı olabilir. Mevcut fotoğraf sayınız:";
        public static string CheckMainCategory = "Bir ana kategori başka bir kategorinin alt kategorisi olamaz.";
        public static string RepeatedData = "Göndermiş olduğunuz veri mevcut. Aynı veriyi tekrarlayamazsınız.";
        public static string CheckSliderAttribute = "Bir özellik hem Ana varyant hem Ana özellik olamaz.";
        public static string DataRuleFail = " Veri belirtilen kurallara uymuyor";
        public static string GetByClaim = "Kullanıcının yetkileri listelendi";
        public static string AuthorizationDenied = "Yetkiniz yok.";
        public static string CurrentMail = "Bu mail adresine sahip bir kullanıcı mevcut.";
        public static string SuccessLogin = "Başarıyla giriş yapıldı";
        public static string UserAvailable = "Böyle bir kullanıcı mevcut";
        public static string AvailableUserMail = "Böyle bir e-mail mevcut başka bir mail adresi giriniz";
        public static string OldPasswordIncorrect = "Eski şifreniz hatalı";
        public static string LoginCheck = "Email ve şifreyi kontrol ediniz";
        public static string UserCheck = "Böyle bir kullanıcı bulunamadı.";
        public static string SuccessRegister = "Başarıyla Kayıt olundu";
        public static string SuccessCreateToken = "Token başarıyal oluşturuldu";
        public static string UnSuccessProductStockCheck = "Sepetteki ürünlerin stoğunu kontrol ediniz.";
        public static string UnSuccessProductStockPrice = "Sepetteki ürünlerin fiyatı eşleşmiyor.";
        public static string failCheckOrder = "Böyle bir siparişiniz bulunmamaktadır.";
        public static string failCancelOrderDate = "Ürün iadesini sipariş ile aynı gün içerisinde verebilirsiniz";
        public static string checkSubOrder = "Sipar içerisinde ürün bulunamadı.";
        public static string PaymentMappingBuyerFail = "Adres işlenirken sorun oluştu";
        public static string PasswordResetCode = "Şifre sıfırlama kodu email adresinize gönderilmiştir";
        public static string FailedPasswordResetCode = "Şifre sıfırlama kodu yanlış.";
        public static string FailedEmailCheck = "Böyle bir mail adresi bulunmamaktadır.";
        public static string CheckStatusError = "Çok fazla hatalı kod girişi yaptınız.";
        public static string ResetPasswordLink = "Şifre sıfırlama linki mail adresinize gönderilmiştir.";
        public static string FindFailedUser = "Kullanıcı bulunamadı";
        public static string SuccessUserPasswordReset = "Şifreniz başarıyla güncellendi";
        public static string UnSuccessUserPasswordReset = "Şifreniz güncellenemedi";
        public static string CodeHasExpired = "Kodun süresi dolmuş. Lütfen tekrardan işlemleri gerçekleştiriniz";
    }
}
