package com.keith.andctr;

import java.util.Date;

import com.keith.androidcontrol.R;

import android.app.Activity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

public class MainActivity extends Activity {

	TextView touchView = null;
	Button Enter = null;
	EditText IP = null;
	EditText Port = null;
	TextView Lbl_IP= null;
	TextView Lbl_Port = null;
	
	String default_ip = "192.168.149.188";
	String default_port = "8818";
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		touchView = (TextView)findViewById(R.id.touchview);
		Enter = (Button)findViewById(R.id.btn_Enter);
		IP = (EditText)findViewById(R.id.txt_IP);
		Port = (EditText)findViewById(R.id.txt_Port);
		
		Lbl_IP = (TextView)findViewById(R.id.textView1);
		Lbl_Port = (TextView)findViewById(R.id.TextView01);
		
		IP.setText(default_ip.toCharArray(),0,default_ip.length());
		Port.setText(default_port.toCharArray(),0,default_port.length());
		
		Enter.setOnClickListener(new ClickListener());
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}
	
	@Override
    public boolean onOptionsItemSelected(MenuItem item) {
        
        if(item.getItemId() == R.id.action_settings){
        	touchView.setText("");
			Enter.setVisibility(View.VISIBLE);
			IP.setVisibility(View.VISIBLE);
			Port.setVisibility(View.VISIBLE);
			Lbl_IP.setVisibility(View.VISIBLE);
			Lbl_Port.setVisibility(View.VISIBLE);
        } 
        return true;
    }
	
	class ClickListener implements OnClickListener{

		@Override
		public void onClick(View v) {
			String ip = IP.getText().toString();
			int port = Integer.parseInt( Port.getText().toString());
			touchView.setOnTouchListener(new TouchListener(ip,port));
			Enter.setVisibility(View.INVISIBLE);
			IP.setVisibility(View.INVISIBLE);
			Port.setVisibility(View.INVISIBLE);
			Lbl_IP.setVisibility(View.INVISIBLE);
			Lbl_Port.setVisibility(View.INVISIBLE);
		}
	
	}
	
	class TouchListener implements OnTouchListener
	{
		long DownTime;
		int DownX,DownY;
		int x,y;
		SocketClient client = null;
		public TouchListener(String ip,int port)
		{
			client = new SocketClient(ip,port);
			client.Connect();
			touchView.setText("Connect server:"+ip+", port:"+port);
		}
		
		@Override
		public boolean onTouch(View v, MotionEvent event) {
			int action = event.getAction();  
            switch (action) {  
            // 当按下的时候   
            case (MotionEvent.ACTION_DOWN):  
            	Display("ACTION_DOWN", event);
                break;  
            // 当按上的时候   
            case (MotionEvent.ACTION_UP):  
            	Display("ACTION_UP", event);
                break;  
            // 当触摸的时候   
            case (MotionEvent.ACTION_MOVE):  
            	Display("ACTION_MOVE", event);
            }  
            return true;  
		}

		public void Display(String eventType, MotionEvent event) {
	         // 触点相对坐标的信息
	         int Rlt_x = (int) event.getX();
	         int Rlt_y = (int) event.getY();
	         // 表示触屏压力大小
	         float pressure = event.getPressure();
	         // 表示触点尺寸
	         float size = event.getSize();
	         // 获取绝对坐标信息
	         int RawX = (int) event.getRawX();
	         int RawY = (int) event.getRawY();
	  
	         String msg = "";
	         msg += "事件类型" + eventType + "\n";
	         msg += "相对坐标" + String.valueOf(x) + "," + String.valueOf(y) + "\n";
	         msg += "绝对坐标" + String.valueOf(RawX) + "," + String.valueOf(RawY) + "\n";
	         msg += "触点压力" + String.valueOf(pressure) + ",";
	         msg += "触点尺寸" + String.valueOf(size) + "\n";
	         
	         String SendMsg="",ReturnMsg="";
	         if	(eventType == "ACTION_DOWN")
	         {
	        	 x = Rlt_x;	y = Rlt_y;
	        	 DownX = Rlt_x;DownY = Rlt_y;
	        	 Date date = new Date();
	        	 DownTime = date.getTime();
	         }
	         
	         if(eventType == "ACTION_MOVE")
	         {
		         int setX = Rlt_x-x;
		         int setY = Rlt_y-y; 
		         SendMsg = "Set " + setX+ " "+ setY;
		         ReturnMsg = client.sendMsg(SendMsg);
		         x = Rlt_x;  y = Rlt_y;
	         }
	         if(eventType == "ACTION_UP")
	         {
	        	 if(x == DownX && y == DownY)
	        	 {
	        		 Date date = new Date();
		        	 long time = date.getTime();
		        	 ReturnMsg = "time= "+ time+" DownTime="+DownTime;
		        	 if(time - DownTime< 500)
		        		 SendMsg = "LClick";
		        	 else
			        	 SendMsg = "RClick"; 
	        		 client.sendMsg(SendMsg);
	        	 }
	         }
	         
	         msg += "SendMsg:" + SendMsg + "\n";
	         msg += "ReturnMsg:" + ReturnMsg + "\n";
	         touchView.setText(msg);
	     }
		
	}

}
