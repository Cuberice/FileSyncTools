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

			List<CoreEquipment> data = DataService.GetAllForModelCache<CoreEquipment>(); //ModelExtensions.CreateInstance<CoreEquipment>);
			Grid.DataSource = data;
		}

		private void GenerateTestData_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = ModelExtensions.CreateTestInstances<CoreEquipment>(5);
			List<CoreEquipmentMake> make = ModelExtensions.CreateTestInstances<CoreEquipmentMake>(5);

			Grid.DataSource = data;
		}

		private void TestSelect_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = DataService.SelectForModel<CoreEquipment>();
			List<CoreEquipment> data2 = DataService.SelectForModel<CoreEquipment>(eq => eq.Name == "4c6fcf40");
		}

		private void TestInsert_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = ModelExtensions.CreateTestInstances<CoreEquipment>(3);
			data.ForEach(i => DataService.InsertModel(i));
			data.Select(i => i.Make).ToList().ForEach(i => DataService.InsertModel(i));
			Grid.DataSource = data;
		}

		private void TestUpdate_Click(object sender, EventArgs e)
		{
			CoreEquipment eq = DataService.GetAllForModel(ModelExtensions.CreateInstance<CoreEquipment>).First();
			eq.SerialNumber = "123-456-789";

			DataService.UpdateModel(eq);

			List<CoreEquipment> equipment = DataService.GetAllForModel(ModelExtensions.CreateInstance<CoreEquipment>);
			Grid.DataSource = equipment;
		}

		private void TestCode_Click(object sender, EventArgs e)
		{
			Guid guid = Guid.Parse("bd006ac3-7583-4c56-96e5-d20265e8f00f");
//			string cmd = DataService.Adapter.CreateSelectCommand<CoreEquipmentMake>(eq => eq.Name == "006583-3");

			string cmd = DataService.Adapter.CreateSelectCommand<CoreEquipmentMake>(eq => eq.ID == guid);
			DataService.Adapter.PerformWithDataReader(cmd, r => ModelExtensions.CreateInstance<CoreEquipmentMake>(r));
		}

		private void TestExpression_Click(object sender, EventArgs e)
		{
			AdapterExtensions.FromExpression<CoreEquipment>(eq => !eq.IsBroken);
			AdapterExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName");
			AdapterExtensions.FromExpression<CoreEquipment>(eq => eq.Cost >= 10.0);
			AdapterExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName" && eq.Cost >= 10.0);
		}
	}
}
