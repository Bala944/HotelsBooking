namespace Booking.Models
{
    public class ConversionHelper
    {
        public static string ToSQLlDatetime(object objDate)
        {
            System.Globalization.DateTimeFormatInfo dateInfoDTMS = new System.Globalization.DateTimeFormatInfo();
            dateInfoDTMS.ShortDatePattern = "dd-MM-yyyy HH:mm:ss";

            DateTime dtValue = new DateTime();
            string sDest = string.Empty;

            try
            {
                dtValue = Convert.ToDateTime(objDate.ToString(), dateInfoDTMS);
                sDest = dtValue.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                
            }

            return sDest;
        }
    }
}
