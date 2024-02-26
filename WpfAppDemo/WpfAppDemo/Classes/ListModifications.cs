using System.Collections.Generic;
using System.Linq;
using WpfAppDemo.Classes;

public static class ListModifications
{
    private const string AscendingOrder = "От А до Я";
    private const string DescendingOrder = "От Я до А";

    public static List<ProductListModel> Order(IEnumerable<ProductListModel> source, string order)
    {
        switch (order)
        {
            case AscendingOrder:
                return source.OrderBy(x => x.ProductName).ToList();
            case DescendingOrder:
                return source.OrderByDescending(x => x.ProductName).ToList();
        }

        return new List<ProductListModel>();
    }

    public static List<ProductListModel> FilterByType(IEnumerable<ProductListModel> source,
        string type)
    {
        return source
            .Where(x => x.ProductType == type)
            .ToList(); 
    }

    public static List<ProductListModel> Search(IEnumerable<ProductListModel> source, string phrase)
    {
        return source.Where(x => x.ProductName.ToLower().Contains(phrase.ToLower())).ToList();
    }
}