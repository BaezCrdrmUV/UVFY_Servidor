using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Logica.Clases;
using Logica.DAO;
using Newtonsoft.Json;

namespace UVFYAndroid
{
	[Activity(Label = "PaginaPrincipalUsuario")]
	public class PaginaPrincipalUsuario : Activity
	{
		private Usuario UsuarioActual { get; set; }
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.main_consumer);
			
			UsuarioActual = new Usuario();
			UsuarioActual.Token = Intent.GetStringExtra("token");
			UsuarioActual.Id = Intent.GetIntExtra("id", 0);
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			Cancion[] cancions = cancionDAO.CargarTodas().Result.ToArray();
			RecyclerView recyclerView = FindViewById<RecyclerView>(Resource.Id.cancionesTodas);
			CancionRecyclerViewAdapter adapter = new CancionRecyclerViewAdapter(cancions);
			RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this);
			recyclerView.SetLayoutManager(layoutManager);
			recyclerView.SetAdapter(adapter);
		}
	}
}