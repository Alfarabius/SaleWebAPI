namespace BuyerAPI
{
    public class Config
    {
        public const string saleApiURL = "https://localhost:5001/SaleAPI";

        public const string buyers = "/Buyers";
        public const string products = "/Products";
        public const string salesPoints = "/SalesPoints";
        public const string sales = "/Sales";

        public const string getBy = "/GetBy/";
        public const string getList = "/GetList";
        public const string create = "/Create";
        public const string deleteBy = "/DeleteBy/";
        public const string updateBy = "/UpdateBy/";

        public const string type = "Basic";
        public const string key = "QWRtaW46QWRtaW4="; // it must be in another secret container, not here. For test only!
    }
}
