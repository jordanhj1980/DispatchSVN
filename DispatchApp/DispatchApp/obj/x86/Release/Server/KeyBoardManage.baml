   M S B A M L   `   `   `  ���� V  RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem F BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp     DispatchApp.KeyBoardManage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf   3system&clr-namespace:System;assembly=mscorlib   %localclr-namespace:DispatchApp      MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���  $	��905�� � 	Resources  �� MergedDictionaries �� Source%  DispatchApp.MemberlistConverter  cvtMemberlist*  $DispatchApp.BoolToIsCheckedConverter  cvtBoolToIschecked$  DispatchApp.TrunklistConverter  cvtTrunklist(  "DispatchApp.BroadcastlistConverter  cvtBroadcast��  �� $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?��� $t npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml?��� $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?��� $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?��� $j dpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.DialogHost.xaml?�%<   &	       &	      &	 
     &	              .��� .+��� $
��1920��  �� DataType  DispatchApp.KeyBoardNew �� ItemsSource �� Path �� Orientation'   !MaterialDesignThemes.Wpf.PackIcon�   �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]	   Kind # MaterialDesignDisplay1TextBlock	  DispatchApp.GroupNew%
   MaterialDesignThemes.Wpf.Badged   ControlzEx.BadgedEx
   Badge �� 	Converter��%�  (��       *, +)7  (�� �     *,	 +)7  �� "   �� $ 	grouplist�.R��� $ 
Horizontal=�.+�  $	 Keyboard=�$��40��$��40��$��0,0,10,0q�$��Center=��� $��SimHei)�$��Center=�8��  ���� $
 name��� " 	 .R��� $ Vertical=�.+��� $ 
Horizontal=�$��0,10,0,0q�.+�
  
 �� *
memberlist+8   .��  $	 AccountSupervisor=�$��40��$��40��$��0,0,10,0q�$��Center=��� $��Center=����� $ 	groupname�.+�#   MaterialDesignThemes.Wpf.Card  +   %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�$��50��  PrimaryHueDarkBrush#���   !	 PrimaryHueDarkForegroundBrush#��� 	    UniformCornerRadius$ 0Q�.���� $��调度键盘��$��35��$��Center=�$��Center=�#��[ ��   
 MaterialDesignLightSeparator#��[
 ]� $	��837��$��Bottom=�$��0,0,0,0q�.+�  -     Name$ keyboardtreeview��$��10,0,0,10.333q�$ Depth5=�$y�Visible=�$��0q�$ 6Q�$��Stretch=�.���� $	��300��$
��Left=�.+��� #��[
 �� $ 
Horizontal=�$��Center=�$	��280��.+��� -    ��Name$ addkey��$��20(�$��添加键盘��$��Microsoft YaHei)�$	��126��$��	10,0,10,0q�$
��Left=��� -   $ delkey��$��20(�$��删除键盘��$��Microsoft YaHei)�$	��126��$��	10,0,10,0q�$��Right=��� #��[
 �� -    ��Name$ scrollViewer��$
��Auto=�$
��Auto=�$	��Top=�$	��505������ $ Vertical=�$
��Auto=�.+�R� -    R�Name$ Keyboardlist������ *KeyboardList+�� -   $��DeskBaseInfo��$��
0,0,0,10.8q�.+�  $ Depth5=�$��40��$
��10,0q�  PrimaryHueLightBrush#���   " PrimaryHueLightForegroundBrush#���  $ 0Q�$��24(�.���� $��基本信息��$��Center=�$��Center=�  -   $ keyboardview��$��
10,0,10,10q�$ Depth5=�$y�Visible=�$��0q�$ 6Q�.���� $��60��.+�]� $��60��.+��� $��	名字：��$��70��$��Center=�$��	40,0,10,0q� # MaterialDesignHeadlineTextBlock#��[ �� $	��272��$��Center=�$��0,0,20,0q�$��24(����� *SelectedKey.name+�� $��IP：��$��36��$��Center=�$��40,16,10,16q�#��[ �� $	��300��$��Center=�$��0,0,20,0q�$��24(����� *SelectedKey.ip+�� $��MAC：��$��70��$��Center=�$��	40,0,10,0q�#��[ �� $	��272��$
��Left=�$��Center=�$��0,0,20,0q�$��24(����� *SelectedKey.mac+  $ Depth3=�$��40��$
��10,0q�#���  #���  $ 0Q�$��24(�.���� $��分组电话��$��Center=�$��Center=��� $��Bottom=�$	��300��.+�]� .+�  -   $ 	groupview��$
��10,0q�$ Depth5=�$y�Visible=�$��0q�$ 0Q�.���� $	��900��$	��300��.+��� .+�]� $��5q�.+��� -	   $��添加分组��$��Microsoft YaHei)�$��20(�$	��126��$��	10,0,20,0q��� -
   $��删除分组��$��Microsoft YaHei)�$��20(�$	��126��$��	10,0,20,0q��� $��Right=�$��	10,0,20,0q�$	��126��$��20(�$��Microsoft YaHei)����� *RunGroupMemberDialogCommand+.��选择成员�� -    ��Name$ grouplistgrid��$��24(�$	��300������ *SelectedKey.grouplist+ �� SelectedItem �� *SelectedGroup+����  �� 
TargetType" ��.R��� $��Disabled=�$
��Auto=�.���  � Orientation$ 
Horizontal=� K� IsItemsHost . ��. ���� .R�  -   $ 0Q�$��5q�$	��140��$��Center=�$��Center=�$��Center=�$��Center=�.���� .+��� .+�]� .+���  % !MaterialDesignSubheadingTextBlock#��[ ]� $��50��.+�
  $��	10,10,0,0q�
 �� *
memberlist+# [  �� Mode$ OneWay=�.��  $	 AccountSupervisor=�$��Bottom=�$
��Left=�$��0,0,10,0q�$��40��$��40���� $��18(�$��10q�$��Center=�$��Center=����� *	groupname+�� #��[
 ]� .+��� $��	描述：��$
��Left=�$��Center=�$
��Bold"�$��5,0,0,10q�#��[ �� $
��Left=�$��Center=�$��	0,0,10,10q�#��[ $
��Wrap=�$	��100������ *description+�� $	��300��.+�  -   $ memberlistcard��$��0,0,10,0q�$ Depth5=�$y�Visible=�$��0q�$ 0Q�$	��300��.���� -   $ scrollAlldev��$
��Auto=�$
��Auto=�$	��Top=�y��� *SelectedGroup+���� $ Vertical=�$
��Auto=�.+�&   System.Windows.Controls.DataGrid  -     Name$ groupdevicegrid��   AutoGenerateColumns .     CanUserAddRows .  $��24(�{�.  ���� *SelectedGroup.memberlist+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 号码��   CanUserReorder .  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *callno+  $ 岗位名称�� .   �� *name+  $ 类型�� .   �� *type+]� $	��316��.+��� .+�  $ Depth5=�$��40��$��0,10,0,0q�#���  #���  $ 0Q�$��24(�$	��286��.���� $ 
Horizontal=�.+��� $��键权电话��$��24(�$��Center=�$
��Left=�$��10,0,0,0q��� $��选择成员��$��20(�$	��145��$��Right=�$��28,0,0,0q�$��Microsoft YaHei)����� *HotlineListViewDialogCommand+  -   $ hotlinedview��$��
10,0,10,10q�$ Depth5=�$y�Visible=�$��0q�$ 0Q�$	��323��$	��286��.���� $��0,0,0,0q�.+��� -   $ hotlinelistgrid��$��24(�$	��320��$��0,0,0,0q����� *SelectedKey.hotlinelist+���� " ��.R��� $��Disabled=�$
��Auto=�.��� $ 
Horizontal=� . ��. ���� .R�  $��5q�$ 0Q�$��60��$��Center=�.���� $ 
Horizontal=�$��Center=�.+�  $	 	Deskphone=�$��10q�$��40��$��40���� $	��240��$��24(�$��5q�$��Center=����� *callno+�� .+�  $ Depth5=�$��40��$��	0,10,10,0q�$y�Visible=�#���  #���  $ 0Q�$��24(�$	��285��.���� $ 
Horizontal=�.+��� $��中继直呼��$��24(�$��Center=�$
��Left=�$��10,0,0,0q��� $��选择成员��$��20(�$��Microsoft YaHei)�$	��145��$��Right=�$��20,0,0,0q����� *TrunklistViewDialogCommand+  -   $ 	trunkview��$��0,0,10,0q�$ Depth5=�$y�Visible=�$��0q�$ 0Q�$	��323��$	��285��.���� $��0,0,0,0q�.+��� -   $ trunklistgrid��$��18(�$	��320��$��0,0,0,0q����� *SelectedKey.trunklist+ �� *SelectedTrunk+���� " ��.R��� $��Disabled=�$
��Auto=�.��� $ 
Horizontal=� . ��. ���� .R�  $��5q�$ 0Q�$	��270��$��60��$��Center=�.���� $ 
Horizontal=�$��Center=�.+�  $	 UserConvert=�$��5q�$��40��$��40���� $	��200��$��24(�$��5q�$��Center=����� *bindingnumber+�� .+�  $ Depth5=�$��40��$��	0,10,10,0q�#���  #���  $ 0Q�$��24(�.���� $ 
Horizontal=�.+��� $��广播分组��$��Center=�$
��Left=�$��10,0,0,0q�  -   $ 	Boardview��$��0,0,10,0q�$ Depth5=�$y�Visible=�$��0q�$ 0Q�$	��323��.���� .+�]� $��5q�.+��� -   $��添加广播��$��20(�$��Microsoft YaHei)�$	��126��$��	10,0,20,0q��� -   $��删除广播��$��20(�$��Microsoft YaHei)�$	��126���� $��Right=�$��	10,0,10,0q�$��20(�$	��126��$��Microsoft YaHei)����� *BroadcastViewDialogCommand+.��选择成员�� -   $ broadcastlistgrid��$��16(�$	��270��$��0,0,0,0q����� *SelectedKey.broadcastlist+ �� *SelectedBroadcast+���� .R��  � 	ItemWidth$	 140��  � 
ItemHeight$	  140������ .R�  -   $��10q�$ 6Q�.���� ! ��Name$! broadcastview��$��10q�$��80��$��80��.+��� $ ��../Resources/broadcast.png��$��50��
  
 �� *bmemberlist+# [ $ OneWay=�.���� $��Right=�$��10,5,0,0q�$��24(����� *name+�� $��60��$��Center=�$ 
Horizontal=�.+��� -   $��更新��$��24(�$
��Left=�$��Center=�$	��100��$��10,0,0,0q��� -   $��result��$��24(�$�� ��$��Center=�$��10,0,0,0q�$	��410��)   #MaterialDesignThemes.Wpf.DialogHost  -   "   
Identifier$" RootDialogHotline��  -   $" RootDialogGroupMember��  -   $" RootDialogTrunk��  -   $" RootDialogBroadcast��  -   $" RootDialogGroupDetail��  -    $" RootDialogTrunkDetail��  -!   $" RootDialogBoardDetail��