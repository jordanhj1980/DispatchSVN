   M S B A M L   `   `   `  ���� F  BDispatchApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DispatchAppDispatchApp      DispatchApp.CreateAlbum   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:DispatchApp     -   ���#444  � ShowInTaskbar  .   � WindowStyle$
 None=� � WindowStartupLocation$ CenterOwner=� � Title$ CreateAlbum��$	��250��$	��430�� � 	Resources    	MainColor �� Color  TextBoxContextMenu m� Command  ColorButton �� 
TargetType �� Property	 �� Value
 �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName��%Z  &	        &	      &	 �     7   7   �� $	 Red���� .@�m� $ ApplicationCommands.Cut��m� $ ApplicationCommands.Copy��m� $ ApplicationCommands.Paste��m� $# ApplicationCommands.SelectAll���� " ��.���  � ��$		 200����  � ��$	 25(���  � ��$	 60����  � ��
	 �������  � ��8	   ��  � ��	 �� "
 ��.R��� $
 back��#��z��8�� $��1q�._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� $
 back�� �@J�Opacity$		 0.8Q�P�  � �� . . ��� $
 back�� � ��$	 99���� $
 back�� � ��$	 34��.���  � RowDefinitions �� $��45 ��� $��* �.+��� $��0��.+��� $��新建歌单��$��18(�$��10q��� ���#A7A7A7$	��0.3q��� $��1��.+��� -   $��CreateAlbumTitle��$��0��
 � Tag$ 20��$��20q�$��40��$��14(�.������ " ��.���  �@�ContextMenu#	 �  ��  � ��	 �� "
 ��.R��� $ border��$
��Auto��$
��Auto��$��1q����#A7A7A7._��  �Name$
 grid�����#FFFFFF.+���  ��Name$ PART_ContentHost��$��Center=�$
��Left=���  ��Name$ x��$y�	Collapsed=����#A7A7A7$��歌单标题��$��Center=�$
��Left=�$��Microsoft YaHei)��� $ x1��$	��5,0q����#A7A7A7#��z $��Center=�$��Right=�$��Microsoft YaHei)� P�  Z�  � ��. ��� $ x�� � y�$	 Visible=�P�  � ��$  ��. ��� $ x�� � y�$	 Visible=� �� Triggers P�  �@J�IsMouseOver . . ���  � ��
	 ���� ��  � ��	 �#444P�  �@J�	IsFocused . . ���  � ��
	 ���� ��  � ��	 �#444P�  � {� .  . ���  � ��
	 ���� ��  � ��	 �#444.���� $��20,20q����#A7A7A7.��设置为隐私歌单��  �� Orientation$ 
Horizontal=�$��Right=�$��20,10q�.+��� -   #��[ $	��100��$��35��$��16(�.��新建�� -   #��[ $��20,0,0,0q�$	��100��$��35��$��16(�
�������#��[  .��取消