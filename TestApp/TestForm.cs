using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Common;
using Core.Data;
using Core.Service;
using Models;
using TestApp.MediaServiceReference;
using TestApp.TestingServiceReference;

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
		protected MediaSyncServiceClient MediaService { get; private set; }

		private void TestForm_Shown(object sender, EventArgs e)
		{
			CoreDataService = new DataService();
			DomainDataService = new MediaSyncService();
			MediaService = new MediaSyncServiceClient();

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
		private void DomainSelect_Click(object sender, EventArgs e)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			List<SyncPath> data = DomainDataService.Data_GetNotWatchedCollection();
			sw.Stop();
			long first = sw.ElapsedMilliseconds;
			
			Grid.DataSource = data;
		}
		private void TestService_Click(object sender, EventArgs e)
		{
//			Service1Client s = new Service1Client();
//			CompositeType ct = s.GetNewDataUsingDataContract();

			int amount = 10;
			List<SyncPath> d = MediaService.Data_GetAllCollectionAmount(amount);
			List<SyncPath> m = MediaService.Data_GetAllCollection();
		}

		private void Insert_Click(object sender, EventArgs e)
		{
			List<SyncPath> data = ModelExtensions.CreateTestInstances<SyncPath>(2000);
			data.ForEach(i => CoreDataService.InsertModel(i));
			data.SelectMany(i => i.Files).ToList().ForEach(i => CoreDataService.InsertModel(i));
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
			List<SyncPath> data = DomainDataService.Data_GetNotSyncedCollection2();
		}

		private void Expression_Click(object sender, EventArgs e)
		{
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName");
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Name == "TestName" && eq.Cost >= 10.0);
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => (eq.Name == "TestName" && eq.Cost >= 10.0) || eq.SerialNumber == "127225-258");

			ExpressionExtensions.FromExpression<CoreEquipment>(eq => eq.Make.Equals(new CoreEquipmentMake()));
			ExpressionExtensions.FromExpression<CoreEquipment>(eq => !eq.IsBroken);
		}
		
		private void DebugAssemblies_Click(object sender, EventArgs e)
		{
			List<Table> data = AttributeExtensions.DebugGetNamespaceTableTypes();

			Grid.DataMember = "TableName";
			Grid.DataSource = data;
		}

		private void Benchmark_Click(object sender, EventArgs e)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			List<SyncPath> data = DomainDataService.Data_GetNotSyncedCollection2();
			sw.Stop();
			long service = sw.ElapsedMilliseconds;
		
			sw.Reset();
			sw.Start();
			List<SyncPath> d = DomainDataService.Data_GetAllCollection();
			sw.Stop();
			long oldtime = sw.ElapsedMilliseconds;
		}
	}
}
