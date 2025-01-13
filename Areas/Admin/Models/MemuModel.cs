namespace Shopping.Areas.Admin.Models
{
    public class MemuModel
    {
        public string title { get; set; }
        public string? controller { get; set; }
        public string? action { get; set; }
        public string? icon { get; set; }
        public List<ItemsModel>? items { get; set; }

    }
    public class ItemsModel
    {
        public string title { get; set; }
        public string? controller { get; set; }
        public string? action { get; set; }
        public string? icon { get; set; }
    }

}
