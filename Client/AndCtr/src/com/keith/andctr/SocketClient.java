package com.keith.andctr;

import java.io.*;
import java.net.*;

 public class SocketClient {
    Socket client;
    
    String site;
    int port;
    
    public SocketClient(String site, int port){
    	this.site = site;
    	this.port = port;
    }
    
    public void Connect()
    {
    	System.out.println("Client Connect! site:"+site+" port:"+port);
    	new Thread(){ 
    		public void run(){ 
	    	        try{
	    	            client = new Socket();
	    	            SocketAddress socAddress = new InetSocketAddress(site, port); 
	    	            client.connect(socAddress, 2000);
	    	            System.out.println("Client is created! site:"+site+" port:"+port);
	    	        }catch (UnknownHostException e){
	    	            e.printStackTrace();
	    	        }catch (IOException e){
	    	            e.printStackTrace();
	    	        }
    			}  
    		}.start();  
    }
    
    public String sendMsg(final String msg){
    	
    	new Thread(){ 
    		public void run(){ 
    			try{
    	            //BufferedReader in = new BufferedReader(new InputStreamReader(client.getInputStream()));
    	            PrintWriter out = new PrintWriter(client.getOutputStream());
    	            out.println(msg);
    	            out.flush();
    	        }catch(IOException e){
    	            e.printStackTrace();
    	        }
    		}
    	}.start();
        
        return "";
    }
    
    public void closeSocket(){
        try{
            client.close();
        }catch(IOException e){
            e.printStackTrace();
        }
    }
    
}