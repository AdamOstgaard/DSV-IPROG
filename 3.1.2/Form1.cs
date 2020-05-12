using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPROG.Uppgifter.uppg3_1_2
{
    public partial class Form1 : Form
    {
        private readonly GuestBookDbConnector _guestBook;

        public Form1(GuestBookDbConnector guestBook)
        {
            InitializeComponent();

            _guestBook = guestBook;
        }

        private async Task UpdateResultsAsync()
        {
            SetLoadingState();

            var results = await _guestBook.GetGuestsAsync();
            resultsView.DataSource = results;

            SetIdleState();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            SetLoadingState();

            await _guestBook.InitializeTablesAsync();
            await UpdateResultsAsync();

            SetIdleState();
        }

        private async void submitButton_click(object sender, EventArgs e)
        {
            SetLoadingState();

            var name = nameInput.Text;
            var email = emailInput.Text;
            var webpage = webpageInput.Text;
            var comment = commentInput.Text;
            var guest = new Guest(name, email, webpage, comment);

            await _guestBook.AddGuestAsync(guest);

            await UpdateResultsAsync();
        }

        private void SetLoadingState()
        {
            Text = "Loading...";
        }

        private void SetIdleState()
        {
            Text = _guestBook.ToString();
        }
    }
}
