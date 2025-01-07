namespace Hsm.Domain.Models.Constants
{
    public static class Constant
    {
        public static class ValidationErrors
        {
            public static string IsNullError(string prop) => $"{prop} property cannot be empty.";
        }

        public static class Table
        {
            public const string Appointments = "Appointment";
            public const string Cities = "Cities";
            public const string Clinical = "Clinicals";
            public const string Doctors = "Doctors";
            public const string Hospitals = "Hospitals";
            public const string Notifications = "Notifications";
            public const string WorkSchedules = "WorkSchedules";
        }

        public static class HtmlBodies
        {
            public static string TakeAppointmnent(string hospitalName, string startDate, string clinicalName, string doctorName)
            {
                return $@"
                <html>
                    <body style='font-family: Arial, sans-serif; color: #333; padding: 20px ;background-color: #f4f4f4;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px;'>
                            <h2 style='color: #2c3e50; text-align: center;'>Randevunuz Alındı</h2>
                            <p style='font-size: 16px; line-height: 1.5;'>Merhaba,</p>
                            <p style='font-size: 16px; line-height: 1.5;'>Aşağıda belirttiğiniz bilgiler doğrultusunda randevunuz başarıyla alınmıştır:</p>
            
                            <table style='width: 100%; margin-top: 20px; border-collapse: collapse;'>
                                <tr style='background-color: #ecf0f1;'>
                                    <td style='padding: 10px; font-weight: bold;'>Hastane:</td>
                                    <td style='padding: 10px;'>{hospitalName}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 10px; font-weight: bold;'>Klinik:</td>
                                    <td style='padding: 10px;'>{clinicalName}</td>
                                </tr>
                                <tr style='background-color: #ecf0f1;'>
                                    <td style='padding: 10px; font-weight: bold;'>Tarih:</td>
                                    <td style='padding: 10px;'>{startDate}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 10px; font-weight: bold;'>Doktor:</td>
                                    <td style='padding: 10px;'>{doctorName}</td>
                                </tr>
                            </table>
            
                            <p style='font-size: 16px; line-height: 1.5; margin-top: 20px;'>Randevunuz başarıyla oluşturulmuştur. Lütfen belirtilen tarihte belirtilen yerde hazır bulunun.</p>
            
                            <p style='font-size: 12px; color: #7f8c8d; text-align: center; margin-top: 30px;'>Bu e-posta bir otomatik mesajdır. Lütfen yanıt vermeyiniz.</p>
                        </div>
                    </body>
                </html>";
            }

            public static string AppointmentAndCancelAppointment(string hospitalName, string startDate, string clinicalName, string doctorName, string appointmentId, string token)
            {
                return $@"
                    <html>
                        <body style='font-family: Arial, sans-serif; padding: 20px ;color: #333; background-color: #f4f4f4;'>
                            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px;'>
                                <h2 style='color: #2c3e50; text-align: center;'>Randevunuz Alındı</h2>
                                <p style='font-size: 16px; line-height: 1.5;'>Merhaba,</p>
                                <p style='font-size: 16px; line-height: 1.5;'>Aşağıda belirttiğiniz bilgiler doğrultusunda randevunuz başarıyla alınmıştır:</p>
            
                                <table style='width: 100%; margin-top: 20px; border-collapse: collapse;'>
                                    <tr style='background-color: #ecf0f1;'>
                                        <td style='padding: 10px; font-weight: bold;'>Hastane:</td>
                                        <td style='padding: 10px;'>{hospitalName}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; font-weight: bold;'>Klinik:</td>
                                        <td style='padding: 10px;'>{clinicalName}</td>
                                    </tr>
                                    <tr style='background-color: #ecf0f1;'>
                                        <td style='padding: 10px; font-weight: bold;'>Tarih:</td>
                                        <td style='padding: 10px;'>{startDate}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; font-weight: bold;'>Doktor:</td>
                                        <td style='padding: 10px;'>{doctorName}</td>
                                    </tr>
                                </table>
            
                                <p style='font-size: 16px; line-height: 1.5; margin-top: 20px;'>Randevunuz başarıyla oluşturulmuştur. Lütfen belirtilen tarihte belirtilen yerde hazır bulunun.</p>
            
                                <p style='font-size: 12px; color: #7f8c8d; text-align: center; margin-top: 30px;'>Bu e-posta bir otomatik mesajdır. Lütfen yanıt vermeyiniz.</p>
                            </div>
                        </body>
                    </html>";
            }

            public static string CancelAppointment(string hospitalName, string startDate, string clinicalName, string doctorName)
            {
                return $@"
                    <html>
                        <body style='font-family: Arial, sans-serif;padding: 20px ; color: #333; background-color: #f4f4f4;'>
                            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px;'>
                                <h2 style='color: #2c3e50; text-align: center;'>Randevunuz İptal Edilmiştir</h2>
                                <p style='font-size: 16px; line-height: 1.5;'>Merhaba,</p>
                                <p style='font-size: 16px; line-height: 1.5;'>Üzgünüz, ancak aşağıda belirtilen randevunuz iptal edilmiştir:</p>
            
                                <table style='width: 100%; margin-top: 20px; border-collapse: collapse;'>
                                    <tr style='background-color: #ecf0f1;'>
                                        <td style='padding: 10px; font-weight: bold;'>Hastane:</td>
                                        <td style='padding: 10px;'>{hospitalName}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; font-weight: bold;'>Klinik:</td>
                                        <td style='padding: 10px;'>{clinicalName}</td>
                                    </tr>
                                    <tr style='background-color: #ecf0f1;'>
                                        <td style='padding: 10px; font-weight: bold;'>Tarih:</td>
                                        <td style='padding: 10px;'>{startDate}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 10px; font-weight: bold;'>Doktor:</td>
                                        <td style='padding: 10px;'>{doctorName}</td>
                                    </tr>
                                </table>
            
                                <p style='font-size: 16px; line-height: 1.5; margin-top: 20px;'>Randevunuz başarıyla iptal edilmiştir. Eğer bu işlemi siz yapmadıysanız lütfen HRYS ile iletişime geçiniz.</p>

                                <p style='font-size: 12px; color: #7f8c8d; text-align: center; margin-top: 30px;'>Bu e-posta bir otomatik mesajdır. Lütfen yanıt vermeyiniz.</p>
                            </div>
                        </body>
                    </html>";
            }
        }
    }
}
