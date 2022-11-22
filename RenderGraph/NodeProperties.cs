using System.Windows.Forms;

namespace RenderGraph
{
    public partial class NodeProperties : Form
    {
        public Node Node { get; set; }

        public NodeProperties()
        {
            InitializeComponent();
        }

        private void NodeProperties_Load(object sender, System.EventArgs e)
        {
            chkLockedPosition.Checked = Node.Locked;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            Node.Locked = chkLockedPosition.Checked;
            DialogResult = DialogResult.OK;
        }
    }
}