using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using logview4net.core;

namespace logview4net.Viewers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SearchForm : Form
    {
        private RichTextBox _txt;
        private int _pos;
        private static string _lastSearch = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchForm"/> class.
        /// </summary>
        /// <param name="txt">The TXT.</param>
        public SearchForm(RichTextBox txt)
        {
            InitializeComponent();
            BackColor = (Color)Logview4netSettings.Instance["BaseColor"];

            _txt = txt;
            _pos = _txt.SelectionStart;
            txtString.Text = _lastSearch;
        }

        private RichTextBoxFinds computeFindOptions(RichTextBoxFinds findOptions)
        {
            if (checkBoxMatchCase.Checked)
            {
                findOptions |= RichTextBoxFinds.MatchCase;
            }
            if (checkBoxWholeWord.Checked)
            {
                findOptions |= RichTextBoxFinds.WholeWord;
            }

            return findOptions;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var findOptions = computeFindOptions(RichTextBoxFinds.None);

            _pos = _txt.Find(txtString.Text, _pos, findOptions) + 1;
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            txtString.Focus();
        }

        private void btnSearchReverse_Click(object sender, EventArgs e)
        {
            var findOptions = computeFindOptions(RichTextBoxFinds.Reverse);

            _pos = _txt.Find(txtString.Text, _pos, findOptions) + 1;
        }
    }
}
