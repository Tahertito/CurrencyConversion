using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HossameProj2
{
    public partial class Form1 : Form
    {
        Dictionary<string, float> concurrencyRate;
        Dictionary<string, float> newConcurrencyRate;
        Dictionary<string, string> concurrencySymbols;
        Int32 index;
        public Form1()
        {
            InitializeComponent();
            concurrencyRate = new Dictionary<string, float>();
            newConcurrencyRate = new Dictionary<string, float>();
            concurrencySymbols = new Dictionary<string, string>();
            index = 0;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                string path = "http://data.fixer.io/api/latest?access_key=f928ca2499a6c787a67647a11ba2f8ab";
                var jobject = await client.GetStringAsync(path);
                var rates = JObject.Parse(jobject);
                concurrencyRate = JsonConvert.DeserializeObject<Dictionary<string, float>>(rates["rates"].ToString());
                comboBox2.DataSource = new BindingSource(concurrencyRate, null);
                comboBox2.DisplayMember = "KEY";
                comboBox2.ValueMember = "VALUE";
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                path = "http://data.fixer.io/api/symbols?access_key=f928ca2499a6c787a67647a11ba2f8ab";
                jobject = await client.GetStringAsync(path);
                rates = JObject.Parse(jobject);
                concurrencySymbols = JsonConvert.DeserializeObject<Dictionary<string, string>>(rates["symbols"].ToString());

                comboBox1.DataSource = new BindingSource(concurrencySymbols, null);
                comboBox1.DisplayMember = "VALUE";
                comboBox1.ValueMember = "KEY";
                ///////////////////////////////////////////////////////////////////////////////////////////////////

                dataGridView1.DataSource = new BindingSource(concurrencyRate.Take(10), null);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;






            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int amount = int.Parse(textBox1.Text);
            var currentsymbolRate =concurrencyRate[comboBox1.SelectedValue.ToString()];
            var valueInEuro = amount / currentsymbolRate;
           
            textBox4.Text=(valueInEuro * float.Parse(comboBox2.SelectedValue.ToString())).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
             index += dataGridView1.Rows.Count;
            if (index < (concurrencyRate.Count/10)*10)
            {
                 newConcurrencyRate = concurrencyRate.OrderBy(c => c.Key).Skip(index).Take(10).ToDictionary(k => k.Key, v => v.Value);
                dataGridView1.DataSource = new BindingSource(newConcurrencyRate, null);
            }
            else if(index == (concurrencyRate.Count / 10) * 10)
            {
                var last = concurrencyRate.Count - ((concurrencyRate.Count / 10) * 10);
                newConcurrencyRate = concurrencyRate.OrderBy(c => c.Key).Skip(index).Take(last).ToDictionary(k => k.Key, v => v.Value);
                dataGridView1.DataSource = new BindingSource(newConcurrencyRate, null);
            }
            else
            {
                index -= dataGridView1.Rows.Count;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            index -= (dataGridView1.Rows.Count );
            if (index >= 0)
            {
                newConcurrencyRate = concurrencyRate.OrderBy(c => c.Key).Skip(index).Take(10).ToDictionary(k => k.Key, v => v.Value);
                dataGridView1.DataSource = new BindingSource(newConcurrencyRate, null);
            }
            else
            {
               
                index = 0;
            }
        }
    }
}
