namespace WebApp.ViewModels
{
    public class JobApplicationSearchVm
    {
        
            public string Q { get; set; }
            public string OrderBy { get; set; }
            public int Page { get; set; }
            public int Size { get; set; }
            public int FromPager { get; set; }
            public int ToPager { get; set; }
            public int LastPage { get; set; }

            public List<ResponseJobApplicationVm>
            Applications
            { get; set; }

    }
}
