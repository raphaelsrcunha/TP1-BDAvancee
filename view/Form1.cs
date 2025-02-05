using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP1_BDAvancee.controller;
using TP1_BDAvancee.model;

namespace TP1_BDAvancee.view
{
    public partial class FormUpdateArticle : Form
    {
        private int articleId;
        private ArticleManager articleController;
        private Form1 parentForm;

        public FormUpdateArticle(int id, Form1 form1)
        {
            InitializeComponent();
            articleController = new ArticleManager();
            articleId = id;
            parentForm = form1;
            LoadArticleDetails();
            buttonTrash.Image = SystemIcons.Error.ToBitmap();
            buttonTrash.ImageAlign = ContentAlignment.MiddleCenter;
        }

        private async void LoadImageFromUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                    HttpResponseMessage response = await client.GetAsync(imageUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Fail to download image. Code: {response.StatusCode}");
                        return;
                    }

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        if (stream == null || stream.Length == 0)
                        {
                            MessageBox.Show("Erro: Stream is void.");
                            return;
                        }

                        using (Image image = Image.FromStream(stream))
                        {
                            pictureBoxArticle.Image = new Bitmap(image);
                            pictureBoxArticle.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error to load image: {ex.Message}");
            }
        }

        private async void pictureBoxArticle_Paint(object sender, PaintEventArgs e)
        {
            Article article = articleController.getArticleById(articleId);

            if (article != null)
            {
                await Task.Run(() => LoadImageFromUrl(article.imageURL));
            }
        }

        private Boolean LoadArticleDetails()
        {
            Article article = articleController.getArticleById(articleId);

            if (article.id != 0)
            {
                textBoxId.Text = article.id.ToString();
                textBoxCode.Text = article.code;
                textBoxName.Text = article.name;
                textBoxDescription.Text = article.description;
                textBoxBrand.Text = article.brand;
                textBoxCategory.Text = article.category;
                textBoxPrice.Text = article.price.ToString();
                textBoxURL.Text = article.imageURL;
                comboBoxValid.Text = article.valid ? "Yes" : "No";
                pictureBoxArticle_Paint(this, null);
                return true;
            }
            return false;
        }

        private void FormUpdateArticle_Load(object sender, EventArgs e)
        {

            if (articleId > 1)
            {
                buttonPreviousUpdate.Enabled = true;
            }
            else
            {
                buttonPreviousUpdate.Enabled = false;
            }

            if (articleId < articleController.getArticlesCount())
            {
                buttonForwardUpdate.Enabled = true;
            }
            else
            {
                buttonForwardUpdate.Enabled = false;
            }
        }

        private void buttonForwardUpdate_Click(object sender, EventArgs e)
        {
            int totalArticles = articleController.getArticlesCount();
            articleId++;

            while (articleId < totalArticles && !LoadArticleDetails())
            {
                articleId++;
            }

            LoadArticleDetails();

            if (articleId <= articleController.getArticlesCount())
            {
                buttonForwardUpdate.Enabled = true;
            }
            else
            {
                buttonForwardUpdate.Enabled = false;
            }

            if (articleId > 1)
            {
                buttonPreviousUpdate.Enabled = true;
            }
            else
            {
                buttonPreviousUpdate.Enabled = false;
            }

        }

        private void buttonPreviousUpdate_Click(object sender, EventArgs e)
        {
            int totalArticles = articleController.getArticlesCount();

            articleId--;
            LoadArticleDetails();

            while (articleId < totalArticles && !LoadArticleDetails())
            {
                articleId--;
            }

            if (articleId > 1)
            {
                buttonPreviousUpdate.Enabled = true;
            }
            else
            {
                buttonPreviousUpdate.Enabled = false;
            }

            if (articleId < articleController.getArticlesCount())
            {
                buttonForwardUpdate.Enabled = true;
            }
            else
            {
                buttonForwardUpdate.Enabled = false;
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                int id = int.Parse(textBoxId.Text);
                string code = textBoxCode.Text;
                string name = textBoxName.Text;
                string description = textBoxDescription.Text;
                string brand = textBoxBrand.Text;
                string category = textBoxCategory.Text;
                decimal price = decimal.Parse(textBoxPrice.Text);
                string url = textBoxURL.Text;
                Boolean valid = comboBoxValid.Text == "Yes" ? true : false;

                Article updatedArticle = new Article(id, code, name, description, brand, category, price, url, valid);
                articleController.updateArticle(updatedArticle);

                MessageBox.Show("Article updated successfully!", "Update Article");

                parentForm.fillDataTableOnlyValids();
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private void buttonTrash_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this image?", "Delete Image", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                pictureBoxArticle.Image = null;
                textBoxURL.Text = "";
                buttonUpdate_Click(sender, e);
            }
        }
    }
}
