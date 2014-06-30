using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core.Data;
using Core.Models;
using Core.Service;
using Models;

namespace MediaSync
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();
		}

		protected DataGridView Grid { get { return grid; } }
		protected IDataService DataService { get; private set; }
		protected IDbAdapter Adapter { get { return DataService.Adapter; } }
		protected SQLBuilder Builder { get; private set; }

		private void TestForm_Shown(object sender, EventArgs e)
		{
			DataService = new DataService();
			Builder = new SQLBuilder(DataService.Adapter);
			Builder.CreateStructure();
		}

		private void GenerateTestData_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = ModelExtensions.CreateTestInstances<CoreEquipment>(5);
			Grid.DataSource = data;
		}

		private void Select_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = DataService.SelectForModel<CoreEquipment>();
			Grid.DataSource = data;
		}
		private void SelectWhere_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = DataService.SelectForModel<CoreEquipment>(eq => eq.Name == txtWhere.Text);
			Grid.DataSource = data;
		}
		private void SelectCache_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = DataService.GetAllForModelCache<CoreEquipment>(); 
			Grid.DataSource = data;
		}

		private void Insert_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = ModelExtensions.CreateTestInstances<CoreEquipment>(3);
			data.ForEach(i => DataService.InsertModel(i));
			data.Select(i => i.Make).ToList().ForEach(i => DataService.InsertModel(i));
			Grid.DataSource = data;
		}

		private void Update_Click(object sender, EventArgs e)
		{
			CoreEquipment eq = DataService.SelectForModel<CoreEquipment>().First();
			eq.SerialNumber = "123-456-789";

			DataService.UpdateModel(eq);

			List<CoreEquipment> equipment = DataService.SelectForModel<CoreEquipment>();
			Grid.DataSource = equipment;
		}

		private void TestCode_Click(object sender, EventArgs e)
		{
			Guid guid = Guid.Parse("bd006ac3-7583-4c56-96e5-d20265e8f00f");
			IAdapterCommand cmd = DataService.Adapter.CreateSelectCommand<CoreEquipmentMake>(eq => eq.ID == guid);
		}

		private void Expression_Click(object sender, EventArgs e)
		{
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName");
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName" && eq.Cost >= 10.0);
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => (eq.Name == "TestName" && eq.Cost >= 10.0) || eq.SerialNumber == "127225-258");

			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Make.Equals(new CoreEquipmentMake()));
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => !eq.IsBroken);
		}

	}
}
