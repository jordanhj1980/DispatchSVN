   M S B A M L   `   `   `  ���� V  RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem F BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp    DispatchApp.SwitchManage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf   3system&clr-namespace:System;assembly=mscorlib   %localclr-namespace:DispatchApp      MaterialDesignBody#���   $��Regular"�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���  $��13(� � 	Resources  �� MergedDictionaries �� Source+  %DispatchApp.BoolToVisibilityConverter   CollapseWhenInvisible  BtVconverter!  DispatchApp.NegateConverter  NegateConverter��  �� $# ../../MaterialDictionary.xaml?�%%   &	       &	         .    .���   �� DataType  DispatchApp.SWDEV �� Orientation'   !MaterialDesignThemes.Wpf.PackIcon�   �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind # MaterialDesignDisplay1TextBlock	 �� Path��%�   (��       *, +)7  �� "  .R��� $ 
Horizontal=�.+�  $ ImportantDevices=�$��40��$��40��$��0,0,10,0q�$��Center=��� $��Center=�8��  ���� $
	 name�.+��� .+�#   MaterialDesignThemes.Wpf.Card  +   %MaterialDesignThemes.Wpf.ShadowAssist
   ShadowDepth$
 Depth5=�$��50��  PrimaryHueDarkBrush#���   ! PrimaryHueDarkForegroundBrush#���     UniformCornerRadius$ 0Q�.���� $��设备管理��$��Center=�$��Center=�$��24(���    MaterialDesignLightSeparator#��[ ]� $	��827��$��Bottom=�.+�  -     Name$ switchtreeview��$��10,0,0,-174.6q�$	��829��$
 Depth5=�$y�Visible=�$��0q�$ 6Q�.���� $	��300��$
��Left=�.+��� #��[ �� $ 
Horizontal=�$��Center=�$	��280��.+��� -    ��Name$ 	addswitch��$��添加设备��$	��128��$��24(�$��	10,0,10,0q�$
��Left=��� -   $ 	delswitch��$��删除设备��$	��128��$��24(�$��	10,0,10,0q�$��Right=��� #��[ �� $
��Auto=�$
��Auto=�$	��Top=�$	��505������ $ Vertical=�$
��Auto=�.+�R� -    R�Name$ 
switchlist������ *
SwitchList+� $��	0,0,0,0.2q� � RowDefinitions �� $��40 ��� $	��130 ��� $��* ��� $
��Auto �.+�  $
 Depth5=�$��40��$
��10,0q� 	 PrimaryHueLightBrush#��� 	  "
 PrimaryHueLightForegroundBrush#��� 
 $ 0Q�$��24(�.���� $��基本信息��$��Center=�$��Center=�  -   $ 
switchview��$��1��$��
10,0,10,10q�$
 Depth5=�$y�Visible=�$��0q�$ 6Q�.���� $	��120��$ Vertical=�.+�]� $��60��.+��� $��	名称：��$��70��$��Center=�$��	40,0,10,0q� # MaterialDesignHeadlineTextBlock#��[ �� -    ��Name$ swName��$	��260��$��Center=�$��0,0,10,0q�$��24(����� *SelectedSwitch.name+ �� 
IsReadOnly �� *	NewSwitch+ �� 	Converter# [ �� $��	类别：��$��70��$��Center=�$��20,16,10,16q�#��[ �� -   $��swType��$	��300��$��Center=�$��0,0,20,0q�$��24(����� *SelectedSwitch.type+�� $��Index:��$y�Hidden=�$��60��$��Center=�$��	20,0,10,0q�#��[ �� -   $��swIndex��$y�Hidden=�$	��300��$
��Left=�$��Center=�$��0,0,20,0q�$��24(����� *SelectedSwitch.index+]� $��60��.+��� $��IP地址：��$��90��$��Center=�$��	20,0,10,0q�#��[ �� -	   $
 swIP��$	��260��$��Center=�$��0,0,10,0q�$��24(����� *SelectedSwitch.ip+�� $��	端口：��$��70��$��Center=�$��20,16,10,16q�#��[ �� -
   $��swPort��$	��300��$��Center=�$��0,0,20,0q�$��24(����� *SelectedSwitch.port+�� $��	描述：��$��70��$��Center=�$��20,16,10,16q�#��[ �� -   $��swDesp��$	��300��$
��Left=�$��Center=�$��0,0,20,0q�$��24(����� *SelectedSwitch.description+]� $��60��  -   $ 	groupview��$��2��$	��600��$��0q�$ 6Q�$
 Depth5=�$	��Top=�$
��10,0q�.���� .+��� .+�  $
 Depth5=�$��40��#��� 	 #��� 
 $ 0Q�$��24(�.���� $��调度用户��$��Center=�$��Center=��� -    ��Name$ 
memberview��$��24(�$	��560������ *SwitchMember+ �� SelectedItem �� *SelectedMember+����  �� 
TargetType" ��.R��� $��Disabled=�$
��Auto=�.���  � Orientation$ 
Horizontal=� K� IsItemsHost . ��. ���� .R�  #���  $��Center=�$��Center=�$��Center=�$��Center=�.���� $��5q�.+��� .+��� $��18(�$��Center=�$��Center=����� *name+�� #��[ �� $��Center=�.+�]� .+��� $��号码:��$��Center=� % !MaterialDesignSubheadingTextBlock#��[ �� $��Center=�$��5,0,10,0q�#��[ ���� *callno+�� $��3��$��60��$��Center=�$ 
Horizontal=�.+��� -   $��保存��$��24(�$��Center=�$	��100��$��10,0,0,0q��� -   $��result��$��24(�$�� ��$��Center=�$��10,0,0,0q�$	��410��)	   #MaterialDesignThemes.Wpf.DialogHost	  -    	  
Identifier$ RootDialogSelectedMember��