using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Logica.Clases;
using Android.Graphics;

namespace UVFYAndroid
{
	class CancionRecyclerViewAdapter : RecyclerView.Adapter
	{
		public event EventHandler<CancionRecyclerViewAdapterClickEventArgs> ItemClick;
		public event EventHandler<CancionRecyclerViewAdapterClickEventArgs> ItemLongClick;
		Cancion[] items;

		public CancionRecyclerViewAdapter(Cancion[] data)
		{
			items = data;
		}

		// Create new views (invoked by the layout manager)
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = null;
			var id = Resource.Layout.CancionEnLista;
			itemView = LayoutInflater.From(parent.Context).
				   Inflate(id, parent, false);

			var vh = new CancionRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
			return vh;
		}

		// Replace the contents of a view (invoked by the layout manager)
		public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
		{
			Cancion item = items[position];

			// Replace the contents of the view with that element
			var holder = viewHolder as CancionRecyclerViewAdapterViewHolder;
			holder.TextViewNombreDeCancion.Text = item.Nombre;
			//holder.imageView.SetImageBitmap(BitmapFactory.DecodeByteArray(item.Imagen, 0, item.Imagen.Length));
		}

		public override int ItemCount => items.Length;

		void OnClick(CancionRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
		void OnLongClick(CancionRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

	}

	public class CancionRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
	{
		public ImageView imageView { get; set; }
		public TextView TextViewNombreDeCancion { get; set; }


		public CancionRecyclerViewAdapterViewHolder(View itemView, Action<CancionRecyclerViewAdapterClickEventArgs> clickListener,
							Action<CancionRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
		{
			TextViewNombreDeCancion = itemView.FindViewById<TextView>(Resource.Id.textViewNombreDeCancion);
			imageView = itemView.FindViewById<ImageView>(Resource.Id.imageView);
			itemView.Click += (sender, e) => clickListener(new CancionRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
			itemView.LongClick += (sender, e) => longClickListener(new CancionRecyclerViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
		}
	}

	public class CancionRecyclerViewAdapterClickEventArgs : EventArgs
	{
		public View View { get; set; }
		public int Position { get; set; }
	}
}