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
		protected IDataService CoreDataService { get; private set; }
		protected IMediaSyncService DomainDataService { get; private set; }
		protected IDbAdapter CoreAdapter { get { return CoreDataService.Adapter; } }
		protected SQLBuilder Builder { get; private set; }

		private void TestForm_Shown(object sender, EventArgs e)
		{
			CoreDataService = new DataService();
			DomainDataService = new MediaSyncService();
			Builder = new SQLBuilder(CoreDataService.Adapter);
			Builder.CreateStructure();
		}

		private void GenerateTestData_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = ModelExtensions.CreateTestInstances<CoreEquipment>(5);
			Grid.DataSource = data;
		}

		private void Select_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = CoreDataService.SelectForModel<CoreEquipment>();
			Grid.DataSource = data;
		}
		private void SelectWhere_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = CoreDataService.SelectForModel<CoreEquipment>(eq => eq.Name == txtWhere.Text);
			Grid.DataSource = data;
		}
		private void SelectNoCache_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = CoreDataService.GetAllForModelNoCache<CoreEquipment>(); 
			Grid.DataSource = data;
		}

		private void Insert_Click(object sender, EventArgs e)
		{
			List<CoreEquipment> data = ModelExtensions.CreateTestInstances<CoreEquipment>(3);
			data.ForEach(i => CoreDataService.InsertModel(i));
			data.Select(i => i.Make).ToList().ForEach(i => CoreDataService.InsertModel(i));
			Grid.DataSource = data;
		}

		private void Update_Click(object sender, EventArgs e)
		{
			CoreEquipment eq = CoreDataService.SelectForModel<CoreEquipment>().First();
			eq.SerialNumber = "123-456-789";

			CoreDataService.UpdateModel(eq);

			List<CoreEquipment> equipment = CoreDataService.SelectForModel<CoreEquipment>();
			Grid.DataSource = equipment;
		}

		private void TestCode_Click(object sender, EventArgs e)
		{
			Guid guid = Guid.Parse("bd006ac3-7583-4c56-96e5-d20265e8f00f");
			IAdapterCommand cmd = CoreDataService.Adapter.CreateSelectCommand<CoreEquipmentMake>(eq => eq.ID == guid);
		}

		private void Expression_Click(object sender, EventArgs e)
		{
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName");
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName" && eq.Cost >= 10.0);
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => (eq.Name == "TestName" && eq.Cost >= 10.0) || eq.SerialNumber == "127225-258");

			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Make.Equals(new CoreEquipmentMake()));
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => !eq.IsBroken);
		}

		private void DomainSelect_Click(object sender, EventArgs e)
		{
			List<SyncPath> data = DomainDataService.Domain_GetAllForSyncPath();
			Grid.DataSource = data;
		}

		private void DebugAssemblies_Click(object sender, EventArgs e)
		{
			List<Table> data = AttributeExtensions.DebugGetNamespaceTableTypes();

			Grid.DataMember = "TableName";
			Grid.DataSource = data;
		}

	}
}
