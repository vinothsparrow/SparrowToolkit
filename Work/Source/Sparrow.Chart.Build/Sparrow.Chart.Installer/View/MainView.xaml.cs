using Sparrow.Resources;
using System.Windows;
using System.Windows.Documents;

namespace Sparrow.Chart.Installer
{   
    public partial class MainView : SparrowWindow
    {       
        public MainView()
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(Sparrow.Chart.Installer.Properties.Resources.license);   
            InitializeComponent();
            this.licenseText.Document.Blocks.Add(paragraph);
        }
      
    }
}