   M S B A M L   `   `   `  ���� V  RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null{Sclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf#MaterialDesignThemes.Wpf.Converters  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem F BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp    DispatchApp.ContactManage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     FmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   rmaterialDesignConvertersSclr-namespace:MaterialDesignThemes.Wpf.Converters;assembly=MaterialDesignThemes.Wpf   3system&clr-namespace:System;assembly=mscorlib   %localclr-namespace:DispatchApp      MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � 	Resources  �� MergedDictionaries �� Source+  %DispatchApp.BoolToVisibilityConverter   CollapseWhenInvisible  BtVconverter!  DispatchApp.NegateConverter  NegateConverter��  �� $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?��� $t npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml?��� $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?��� $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?��� $j dpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.DialogHost.xaml?�%%   &	       &	         .    .���   �� DataType  DispatchApp.Department �� Orientation'   !MaterialDesignThemes.Wpf.PackIcon�   �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind # MaterialDesignDisplay1TextBlock	 �� Path��%�   (��       *, +)7  �� "  .R��� $ 
Horizontal=�.+�  $ ContactMail=�$��40��$��40��$��0,0,10,0q�$��Center=��� $��Center=�8��  ���� $	 
department�.+��� .+�#   MaterialDesignThemes.Wpf.Card  +   %MaterialDesignThemes.Wpf.ShadowAssist
   ShadowDepth$
 Depth5=�$��50��  PrimaryHueDarkBrush#���   ! PrimaryHueDarkForegroundBrush#���     UniformCornerRadius$ 0Q�.���� $��电话簿管理��$��35��$��Center=�$��Center=�#��[ ��    MaterialDesignLightSeparator#��[ ]� $	��825��$��Bottom=�.+�  -     Name$ contacttreeview��$��10,0,0,-150.6q�$	��822��$
 Depth5=�$y�Visible=�$��0q�$ 6Q�.���� $	��300��$
��Left=�.+��� #��[ �� $ 
Horizontal=�$��Center=�$	��280��.+��� -    ��Name$ addgroup��$��添加分组��$	��128��$��24(�$��	10,0,10,0q�$
��Left=��� -   $ delgroup��$��删除分组��$	��128��$��24(�$��	10,0,10,0q�$��Right=��� #��[ �� $
��Auto=�$
��Auto=�$	��Top=�$	��505������ $ Vertical=�$
��Auto=�.+�R� -    R�Name$ contactlist������ *ContactList+�  � RowDefinitions �� $��40 ��� $��60 ��� $��* ��� $
��Auto �.+�  $
 Depth5=�$��40��$
��10,0q� 	 PrimaryHueLightBrush#��� 	  "
 PrimaryHueLightForegroundBrush#��� 
 $ 0Q�$��24(�.���� $��基本信息��$��Center=�$��Center=�  -   $ userview��$��1��$��
10,0,10,10q�$
 Depth5=�$y�Visible=�$��0q�$ 6Q�.���� $��60��$ Vertical=�.+�]� $��60��.+��� $��	名称：��$��70��$��Center=�$��40,0,0,0q� # MaterialDesignHeadlineTextBlock#��[ �� -    ��Name$ deptName��$	��260��$��Center=�$
��Left=�$��24(����� *SelectedContact.department+ �� Mode$ OneWay=� �� 
IsReadOnly �� *	NewUser+ �� 	Converter# [   -   $��contactCard��$��2��$
 Depth3=�$	��660��$
��10,0q�$ 0Q�$��24(�.����  J� RenderTransformOrigin$ 0.458,0.499&�$ 
Horizontal=�.+�&	   System.Windows.Controls.DataGrid	  -   $��gridMem��
������ $��0��$��24(� 	  AutoGenerateColumns .   	  CanUserAddRows . $��	0,0,0,0.4q�$��Hidden=�" 	  VerticalScrollBarVisibility$
 Auto=�$	��700������ *gridMemList+ =
  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value  ColumnElementStyle��%�   '
        &	 o     �� " 
 .���  � ��$ Center=���  � ��$ Center=���  � ��
 ���� ��  � ��$ 50���� " ��.���  � ��$ 80,0,0,0q� 	  Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$	 350    Header$ 电话号码��1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [    Binding �� *callno+  $	 350 $ 标识��# [  �� *name+�� $��3��$��60��$��Center=�$ 
Horizontal=�.+��� -	   $��delBtn��$��删除��$��24(�$��Center=�$	��100��$��	10,0,10,0q��� -
   $��保存��$��24(�$��Center=�$	��100��$��10,0,0,0q��� -   $��result��$��24(�$�� ��$��Center=�$��10,0,0,0q�$	��410��