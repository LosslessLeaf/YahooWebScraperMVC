using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace YahooWebScraperMVC_Auth
{
    public class SQL
    {
        public SqlConnectionStringBuilder Connection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "LAPTOP-SPSCMI79";
            builder.InitialCatalog = "YahooScraper";
            builder.IntegratedSecurity = true;

            return builder;
        }

        public void StoreData(Stock stock, SqlConnectionStringBuilder builder)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO tblFinanceData (Symbol, LastPrice, Change, ChangePercentage, Currency, MarketTime, Volume, AvgVolume, MarketCap) VALUES ('" +
                                    stock.Symbol + "', '" + stock.LastPrice + "', '" + stock.Change + "', '" + stock.ChangePercentage + "', '" + stock.Currency + "', '" + stock.MarketTime + "', '" + stock.Volume + "', '" + stock.AvgVolume + "', '" + stock.MarketCap + "')";

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<Stock> RetrieveData(SqlConnectionStringBuilder builder, int RetrievalAmount)
        {
            List<Stock> StockList = new List<Stock>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TOP " + RetrievalAmount + " [StockID], [Symbol], [LastPrice], [Change], [ChangePercentage], [Currency], [MarketTime], [Volume], [AvgVolume], [MarketCap], [TimeScraped] FROM [YahooScraper].[dbo].[tblFinanceData] ORDER BY StockID DESC";
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Stock stock = new Stock();
                        stock.Symbol = reader["Symbol"].ToString();
                        stock.LastPrice = reader["LastPrice"].ToString();
                        stock.Change = reader["Change"].ToString();
                        stock.ChangePercentage = reader["ChangePercentage"].ToString();
                        stock.Currency = reader["Currency"].ToString();
                        stock.MarketTime = reader["MarketTime"].ToString();
                        stock.Volume = reader["Volume"].ToString();
                        stock.AvgVolume = reader["AvgVolume"].ToString();
                        stock.MarketCap = reader["MarketCap"].ToString();

                        StockList.Add(stock);
                    }

                    connection.Close();
                }
            }

            return StockList;
        }


        //SQL SQL = new SQL();
        //SqlConnectionStringBuilder builder = SQL.Connection();
        //SQL.StoreData(stock, builder);
    }

}
