using Blackbird.Applications.Sdk.Common;

namespace Apps.Voiseed.Models
{
    public class GlossaryDto
    {
        [Display("Glossary ID")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display("Created at")]
        public DateTime CreatedAt { get; set; }

        [Display("Updated at")]
        public DateTime UpdatedAt { get; set; }
    }
}
