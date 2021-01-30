package crc643174fce518eee543;


public class DiaryRecyclerViewAdapterViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("A1CPro.DiaryRecyclerViewAdapterViewHolder, A1CPro", DiaryRecyclerViewAdapterViewHolder.class, __md_methods);
	}


	public DiaryRecyclerViewAdapterViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == DiaryRecyclerViewAdapterViewHolder.class)
			mono.android.TypeManager.Activate ("A1CPro.DiaryRecyclerViewAdapterViewHolder, A1CPro", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
