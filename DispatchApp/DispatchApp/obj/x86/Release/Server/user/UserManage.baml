   M S B A M L   `   `   `  ���� V  RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem F BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp    DispatchApp.UserManage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf   3system&clr-namespace:System;assembly=mscorlib   %localclr-namespace:DispatchApp      MaterialDesignBody#���   $��Regular"�$��24(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � 	Resources  �� MergedDictionaries �� Source+  %DispatchApp.BoolToVisibilityConverter   CollapseWhenInvisible  BtVconverter!  DispatchApp.NegateConverter  NegateConverter��  �� $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?��� $t npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml?��� $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?��� $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?��� $j dpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.DialogHost.xaml?�%%   &	       &	         .    .���  2  ,DispatchApp.MyPrivilegeToVisibilityConverter  MyValToVisibilityConverter$ DispatchApp.UserImageConverter  ImgConverter �� DataType  DispatchApp.User �� Orientation �� Path	 �� 	Converter # MaterialDesignDisplay1TextBlock��%�   &	       &	      (�� 
     *, +)7  7      �� "  .R��� $ 
Horizontal=�.+��� $��40��$��40��$��0,0,10,0q�$��Center=����� $ 	privilege�8	   �� $��Center=�8�� ���� $
 name�
 � ColumnDefinitions
 �� $
��643* ��� $
��317* �.+��� $��2��.+�#   MaterialDesignThemes.Wpf.Card  +   %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�$��50��  PrimaryHueDarkBrush#���   !	 PrimaryHueDarkForegroundBrush#��� 	    UniformCornerRadius$ 0Q�.���� $��用户管理��$��Center=�$��Center=�$��24(���   
 MaterialDesignLightSeparator#��[
 ]� $	��832��$��Bottom=�.+�  -     Name$ usertreeview��$��10,0,0,-172.6q�$	��827��$ Depth5=�$y�Visible=�$��0q�$ 6Q�.���� $	��300��$
��Left=�.+��� #��[
 �� $ 
Horizontal=�$��Center=�$	��280��.+��� -    ��Name$ adduser��$��添加用户��$	��128��$��24(�$��	10,0,10,0q�$
��Left=��� -   $ deluser��$��删除用户��$	��128��$��24(�$��	10,0,10,0q�$��Right=��� #��[
 �� $
��Auto=�$
��Auto=�$	��Top=�$	��505������ $ Vertical=�$
��Auto=�.+�R� -    R�Name$ 	adminlist������ *	AdminList+�� #��[
 �� $
��Auto=�$
��Auto=�$	��Top=�$	��505������ $ Vertical=�$
��Auto=�.+�R� -   $ userlist������ *
UserList+�  � RowDefinitions �� $��40 ��� $��* ��� $
��Auto �.+�  $ Depth5=�$��40��$
��10,0q�  PrimaryHueLightBrush#���   " PrimaryHueLightForegroundBrush#���  $ 0Q�$��24(�.���� $��基本信息��$��Center=�$��Center=�  -   $ userview��$��1��$��
10,0,10,10q�$ Depth5=�$y�Visible=�$��0q�$ 6Q�.���� $	��180��$ Vertical=�.+�]� $��60��.+��� $��	名字：��$��70��$��Center=�$��	40,0,10,0q� # MaterialDesignHeadlineTextBlock#��[ �� -    ��Name$ userName��$	��260��$��Center=�$��0,0,10,0q�$��24(����� *SelectedUser.name+ �� 
IsReadOnly �� *	NewUser+#	 [ �� $��	密码：��$��70��$��Center=�$��20,16,10,16q�#��[ �� -   $ userPass��$	��300��$��Center=�$��0,0,20,0q�$��24(����� *SelectedUser.password+�� $��	状态：��$��70��$��Center=�$��Right=�$��	90,0,10,0q�$��24(��� -	   $ 
userStatus��$	��260��$
��Left=�$��Center=�$��0,0,20,0q�$��24(����� *SelectedUser.status+]� $��60��.+��� $��	权限：��$��70��$��Center=�$��	40,0,10,0q�#��[ �� -
    ��Name$ userPriv��$	��260��$
��Left=� �� SelectedValuePath$ id�� �� DisplayMemberPath$ description��$��Center=�$��0,0,10,0q�$��24(����� *privCandidateList+ �� SelectedValue �� *SelectedUser.privilege+ �� Mode$ OneWay=��� $��	角色：��$��70��$��Center=�$��20,16,10,16q�#��[ �� -   $ userRole��$	��300��$��Center=�$��0,0,20,0q�$��24(����� *SelectedUser.role+�� $��所属调度台：��$	��140��$��Center=�$��	20,0,10,0q�#��[ y��� *SelectedValue+#	 [  �� ElementName$ userPriv��$ OneWay=��� -   $ userDesk��$	��260��$
��Left=�$ id��$ description��$��Center=�$��0,0,20,0q�$��24(����� *
deskList+y��� *SelectedValue+#	 [ $ userPriv��$ OneWay=� �� *SelectedUser.desk+$ OneWay=�]� $��60��.+��� $��	描述：��$��70��$��Center=�$��40,16,10,16q�#��[ �� -   $ userDesp��$	��260��$
��Left=�$��Center=�$��0,0,20,0q�$��24(����� *SelectedUser.description+�� $��2��$��60��$��Center=�$ 
Horizontal=�.+��� -   $��保存��$��24(�$��Center=�$	��100��$��10,0,0,0q��� -   $��result��$��24(�$�� ��$��Center=�$��10,0,0,0q�$	��410��