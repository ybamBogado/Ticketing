using Microsoft.EntityFrameworkCore;

namespace Application.Queries
{
    public class GetEventCatalogQuery 
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}