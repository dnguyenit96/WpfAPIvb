   M S B A M L   `   `   `  ���� C  ?WpfAPIvb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WpfAPIvbWpfAPIvb      WpfAPIvb.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  "localclr-namespace:WpfAPIvb  -   $	��700��$
��1152��  � WindowStartupLocation$  CenterScreen=�$��Times New Roman)�$
��Bold"� � WindowStyle$
 None=� � Icon$ Json.ico�� � 
ResizeMode$ CanMinimize=� � AllowsTransparency . ���	#FFDFE9FF���	#FF3F51B5$��1,0,1,1q� � 	Resources  �� 
TargetType �� Property �� Value��%E   '��       �� " ��.���  � ��$
 Bold"���  � ��$ 13(�.��� 	 � RowDefinitions	 �� $��40 ��� $	��660 �
 � ColumnDefinitions
 �� $	��50* ��� $	��50* �.+�#  MaterialDesignThemes.Wpf.Card  $��0��$��0��$��2��+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth3=�   ShadowEdges$ Bottom=����	#FF3F51B5.��� -    �Name$ gridTitleBarLeft�����	#FF3F51B5.+���  �� Orientation$ 
Horizontal=�$
��Left=�$��8,0,0,0q�.+��� $��FSD Tool API��$��Center=�$
��Bold"�$��24(����#FFFFFF� -   $ gridTitleBarRigt��$��0��$��1�����	#FF3F51B5.+��� $ 
Horizontal=�$��Right=�.+��� -    ��Name$ btnMinimizeForm��$��32��$��32�� &  "MaterialDesignFloatingActionButton#��[  ���#FFFFFF$��0,0,8,0q���Z� ��Z� .��'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ WindowMinimize=��� -   $ btnCloseForm��$��32��$��32��#��[   � ToolTip$ Tắt ứng dụng�����#FFFFFF$��0,0,8,0q���Z� .��  $ Power=�� -   $ girdMainLeft��$��1��$��0��$��8,0,4,8q�	 �� $��32 ��� $	��438 ��� $��32 ��� $	��150 �.+��� $
��Json��$��0��$��0,8,0,0q��� -    ��Name$ txtJson��$��1�� * &MaterialDesignOutlinedTextFieldTextBox#��[ $	��438�����#FFFFFF$��0��$	��Top=� �� AcceptsReturn .  �� TextWrapping$
 Wrap=�$��16(�$��Medium"�" �� VerticalScrollBarVisibility$
 Auto=��� $��Result��$��2��$��0,8,0,0q��� -   $ 	txtResult��$��3��#��[ $	��150�����#FFFFFF$��1��$	��Top=� . $
 Wrap=�$��16(�$��Medium"�$
 Auto=����� *Result+� -	   $ GirdMainRight��$��1��$��1��$��4,0,8,0q�	 �� $	��295 ��� $	��294 ��� $��56 �.+�� $��0��.+�]� ��. .+��� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,8,0,0q�.+��  � Orientation$ 
Horizontal=�$	��100��.+��� $��Link API���� $��*��
���  ���� -
   $ 
txtLinkAPI��  MaterialDesignTextBoxBase#��[ $��Medium"�$��16(�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ http://172.168.5.14\API��$	��460��$��2��.���� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+�� $ 
Horizontal=�$	��100��.+��� $��Source���� $��*��
���  ���� -   $ 	txtSource��#��[ $��Medium"�$��16(�$ \\172.168.5.14��$	��460��$��3���� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+�� $ 
Horizontal=�$	��100��.+��� $��Private Key���� $��*��
���  ���� -   $ txtPrivateKey��#��[ $��Medium"�$��16(�$ BHAJKHJK��$	��330��$��4��� $ 
Horizontal=�.+��� $��User ID���� $��*��
���  ���� -   $ 	txtUserID��#��[ $��Medium"�$��16(�$ 21��$��64�� �� 	MaxLength$ 3��$��5���� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+��� $��
Controller��$	��100���� -    ��Name$ cbxController�� �� SelectedValuePath$ FileXMLName��$	��330��$��6��-  'MaterialDesignThemes.Wpf.ComboBoxAssist   ClassicMode$
 True��$ getItems������ .R��� $��16(�$��Medium"����� *FileXMLName+�� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+��� $
��Time��$	��100���� -   $ txtTime��#��[ $��Medium"�$��16(�$	��330��$��7���� -    ��Name$ chkTime��$
��auto�� d� 	IsChecked$
 TrueY�$��8��   MaterialDesignCheckBoxOff �� Color  PrimaryHueMidBrush��%<   &	       &	      �� $ #000000���� $ 	#FF3F51B5���� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+��� $
��Form��$	��100���� -   $ txtForm��#��[ $��Medium"�$��16(�$	��330��$��9���� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+��� $	��MD5��$	��100���� -   $ txtMD5��#��[ $��Medium"�$��16(�$	��330��$��10���� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+��� $	��Key��$	��100���� -   $ txtKey��#��[ $��Medium"�$��16(�$	��330��$��11���� $	��Top=�$
��Left=�$ 
Horizontal=�$��0,5,0,0q�.+��� $��Token��$	��100���� -   $ txtToken��#��[ $��Medium"�$��16(�$	��330��$��12��� $��1��	 �� $��24 ��� $	��270 �.+��� $
��Post��$��0���� -   $ txtPost��$��1��#��[ $	��270�����#FFFFFF$��13��$	��Top=� . $
 Wrap=�$��16(�$��Medium"�$
 Auto=����� *Post+� $��2��$��0,0,0,0q�
 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   $ btnLogin��$��0��$	��130��$��40��  MaterialDesignRaisedButton#��[ $ Đăng nhập dự án��+  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 20��$ Depth3=�$��0,0,8,0q�$��13��.���� $ 
Horizontal=�.+��� $��LOGIN��$
��Bold"�  $��4, 0.5, 0, 0q�$ Login=�
���  ���� -   $ btnPost��$��1��$	��130��$��40��#��[ $ Gửi dữ liệu��$ 20��$ Depth3=�$��0,0,8,0q�$��14��.���� $
��POST��$
��Bold"��� -   $ btnDescrypt��$��2��$	��130��$��40��#��[ $ 
Giải mã��$ 20��$ Depth3=�$��0,0,8,0q�$��15��.���� $��DESCRYPT��$
��Bold"��� -   $ btnEtoJ��$��3��$	��130��$��40��#��[ $' !Dán clip board excel thành json��$ 20��$ Depth3=�$��0,0,8,0q�$��16��.���� $ 
Horizontal=�.+�  $ 	FileExcel=�$��	0,0.5,4,0q����	#FF1FBD66�� to  $ CodeJson=�$��	4,0.5,0,0q�