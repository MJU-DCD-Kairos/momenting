package com.unity3d.player
import android.app.Activity
import android.content.Context
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.FrameLayout
import android.widget.Toast
import com.unity3d.player.UnityPlayer
import com.unity3d.player.UnityPlayerActivity

class SubActivity : UnityPlayerActivity(){

    fun SubFun(mContext : Context, activity_sub_id : Int,send_et_id: Int, send_btn_id: Int){
        val inflater:LayoutInflater = mContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
        val  ll:FrameLayout=inflater.inflate(activity_sub_id,null)as FrameLayout
        val paramll = FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT,FrameLayout.LayoutParams.MATCH_PARENT)
        (mContext as Activity).addContentView(ll,paramll)

        val send_et_view:EditText = mContext.findViewById<EditText>(send_et_id)
        val send_btn_view:Button = mContext.findViewById<Button>(send_btn_id)

        send_et_view.addTextChangedListener(object  : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}
            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int){
                if(s.toString() != "") send_btn_view.visibility = View.VISIBLE
                else send_btn_view.visibility = View.GONE
            }
            override fun afterTextChanged(s: Editable?) {}
        })

        send_et_view.setOnClickListener{
            UnityPlayer.UnitySendMessage("GameManager","ReceiveMessage",send_et_view.text.toString())
            send_et_view.setText("")
        }

    }
}