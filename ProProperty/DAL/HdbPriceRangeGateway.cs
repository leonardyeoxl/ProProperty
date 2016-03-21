using ProProperty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProProperty.DAL
{
    public class HdbPriceRangeGateway : DataGateway<Hdb_price_range>
    {
        public List<Hdb_price_range> hdbPriceRangeQuery(string district, string room)
        {
            return data.Where(p => p.town == district && p.room_type == room).ToList();
        }



        //List<Hdb_price_range> tempList = data.Where(p => p.town == district && p.room_type == room).ToList();

        /*List<Hdb_price_range> confirmedList = new List<Hdb_price_range>();

        string financialYearGroup = tempList.ElementAt(0).financial_year;

        double min_selling_priceTotal = 0.0;
        double max_selling_priceTotal = 0.0;

        int counter = 0;

        for (int i=0; i<tempList.Count; i++)
        {
            if (tempList.ElementAt(i).financial_year == financialYearGroup)
            {
                counter++;
                // 2008 $1234
                // 2008 $1111
                // 2008 $1122
                // average for 2008 $2719

                min_selling_priceTotal += Convert.ToDouble(tempList.ElementAt(i).min_selling_price);
                max_selling_priceTotal += Convert.ToDouble(tempList.ElementAt(i).max_selling_price);

            }
            else
            {
                double average_min_selling_price = min_selling_priceTotal / counter;
                double average_max_selling_price = max_selling_priceTotal / counter;

                Hdb_price_range newHdbPriceRangeModel = new Hdb_price_range();
                newHdbPriceRangeModel.financial_year = financialYearGroup;
                newHdbPriceRangeModel.max_selling_price = Convert.ToString(average_max_selling_price);
                newHdbPriceRangeModel.min_selling_price = Convert.ToString(average_min_selling_price);
                newHdbPriceRangeModel.room_type = room;
                newHdbPriceRangeModel.town = district;

                confirmedList.Add(newHdbPriceRangeModel);

                //set to default
                financialYearGroup = tempList.ElementAt(i).financial_year;
                min_selling_priceTotal = 0.0;
                max_selling_priceTotal = 0.0;
                counter = 0;
                //set to default

                counter++;

                min_selling_priceTotal += Convert.ToDouble(tempList.ElementAt(i).min_selling_price);
                max_selling_priceTotal += Convert.ToDouble(tempList.ElementAt(i).max_selling_price);

            }
        }*/

    }
}