using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Logica.Clases;
using Logica.ClasesDeComunicacion;
using Logica.DAO;
using Newtonsoft.Json;

namespace UVFYAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Button buttonIniciarSesion = FindViewById<Button>(Resource.Id.buttonIniciarSesion);
            buttonIniciarSesion.Click += ButtonIniciarSesionOnClickAsync;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private async void ButtonIniciarSesionOnClickAsync(object sender, EventArgs eventArgs)
        {
			Usuario usuario = new Usuario()
			{
				CorreoElectronico = "Pachy@correo.com",
				Contraseña = "perros"
			};
			UsuarioDAO usuarioDAO = new UsuarioDAO();
			RespuestaDeAutenticacion respuesta = new RespuestaDeAutenticacion();
			try
			{
				respuesta = await usuarioDAO.ValidarUsuario(usuario);
			}
			catch (Exception ex)
			{
				Toast.MakeText(ApplicationContext, ":( " + ex.Message + ex.InnerException, ToastLength.Long).Show();
				return;
			}

			if (respuesta.Response)
			{
				usuario.Token = respuesta.Token;
				usuario.TipoDeUsuario = respuesta.TipoDeUsuario;
				usuario.Id = respuesta.IdUsuario;
				if (respuesta.TipoDeUsuario == TipoDeUsuario.Consumidor)
				{

					var pantallaPrincipalDeConsumidor = new Intent(this, typeof(PaginaPrincipalUsuario));
					Intent.PutExtra("token", usuario.Token);
					Intent.PutExtra("id", usuario.Id.ToString());
					StartActivity(pantallaPrincipalDeConsumidor);

				}
				else if (respuesta.TipoDeUsuario == TipoDeUsuario.Artista)
				{

				}
				else
				{
					Toast.MakeText(ApplicationContext, "Error, usuario indefinido" + respuesta.Token + " " + respuesta.IdUsuario, ToastLength.Long).Show();

				}
			}
			else
			{
				Toast.MakeText(ApplicationContext, "Correo electronico o contraseña invalidos", ToastLength.Long).Show();
			}
		}
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}

