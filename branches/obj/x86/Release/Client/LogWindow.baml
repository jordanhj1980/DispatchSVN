   M S B A M L   `   `   `  ����   DispatchApp    DispatchApp.LogWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     $	��757��$
��1499��
���   �  � 	Resources   �� MergedDictionaries �� Source��  �� $< 6/DispatchApp;component/Server/ManageStyleResource.xaml?�%    .��� $	��757��$��Center=�$	��Top=�$
��1499��$��0,0,3.333,0q�.+��� $��日志��$��0,0,0,0q����	#FFF3ECEC$��20(��� -   $��24(�$��SimHei)�$
��Left=�$��58��$��1446,0,-5,726q� J� RenderTransformOrigin$ -8.393,0.368&���Z� ��Z� .���� $!��../Resources/closeWhite.png��&   System.Windows.Controls.DataGrid  -   $
��Date��$��69��$��12(�$��10,47,1198,604q�$	��296��
���   ���Z�    Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$	 148    Header$ 起始时间��   CellTemplate ��    CellEditingTemplate �� .R�(  "System.Windows.Controls.DatePicker  	   SelectedDate	 �� 
 �� Path$
 	DateAdded� �� Mode$ TwoWay=�  $	 148 $ 终止时间�� ��  �� .R�  	 �� $
 	DateAdded�$ TwoWay=��� $��通话日志��$��10,260,707,289q����	#FFF9F4F4�� $��故障日志��$��10,327,704,220q����	#FFF9F4F4  -   $��	DetialMsg��
���   �$	��456��$
��1166��$��Center=�$	��Top=�$��328,31,10,0q�
������� 0  *System.Windows.Controls.DataGridTextColumn  $	 145 $ 话单��   
Foreground
 �����1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Cdrid+  $	 145 $ 唯一标识��
 ����� �� *callid+  $	 145 $ 通话类型��
 ����� �� *type+  $	 145 $ 呼叫起始时间��
 ����� �� *	TimeStart+  $	 145 $ 呼叫结束时间��
 ����� �� *	TimeEnd+  $	 145 $ 主叫号码��
 ����� �� *CPN+  $	 145 $ 被叫号码��
 ����� �� *CDPN+  $	 151 $ 通话时长��
 ����� �� *
Duration+�� -   $��button1��$��确定��$��23��$��75��$��348,501,367,46q��� -    ��Name$ button3��$��保存��$��23��$��586,576,832,121q�$ 	0.5,0.507&��� -   $��button2��$��清除��$��23��$��75��$��460,501,254,46q�