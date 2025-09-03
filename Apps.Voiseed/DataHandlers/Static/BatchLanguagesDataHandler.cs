using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.DataHandlers.Static
{
    public class BatchLanguagesDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData()
        {
            return new List<DataSourceItem>
            {
                new("AR-AR", "Arabic"),
                new("BG-BG", "Bulgarian"),
                new("DA-DK", "Danish"),
                new("DE-DE", "German"),
                new("EL-GR", "Greek"),
                new("EN-GB", "English (UK)"),
                new("EN-US", "English (US)"),
                new("ES-ES", "Spanish (Spain)"),
                new("ES-LA", "Spanish (Latin America)"),
                new("FI-FI", "Finnish"),
                new("FR-FR", "French"),
                new("HI-IN", "Hindi"),
                new("ID-ID", "Indonesian"),
                new("IT-IT", "Italian"),
                new("JA-JP", "Japanese"),
                new("KO-KR", "Korean"),
                new("NB-NO", "Norwegian"),
                new("NL-NL", "Dutch"),
                new("PL-PL", "Polish"),
                new("PT-PT", "Portuguese (Europe)"),
                new("PT-BR", "Portuguese (Brazil)"),
                new("SV-SE", "Swedish"),
                new("TR-TR", "Turkish"),
                new("ZH-CN", "Chinese (Mandarin)")
            };
        }
    }
}
