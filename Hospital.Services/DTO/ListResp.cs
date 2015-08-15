using System.Collections.Generic;

namespace Hospital.Services.DTO
{
    public class ListResp<T>
    {
        public ListResp(IList<T> items)
        {
            Items = items;
            if (items != null)
            {
                Total = items.Count;
            }
        }

        public ListResp()
        {
            Items = new List<T>();
        }

        public IList<T> Items { get; set; }
        public int Total { get; set; }
    }
}