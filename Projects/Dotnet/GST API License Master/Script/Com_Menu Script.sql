Declare @rrange Numeric,@maxRange Numeric

IF NOT EXISTS (SELECT [PADNAME],[BARNAME] FROM COM_MENU WHERE padname='MAINMASTERS' AND barname='GSPAPILicenseMaster')
BEGIN
	Select @maxRange=max(Range)+1 From com_menu
	Execute [USP_Chk_ComMenuRange] @maxRange,@rrange= @rrange Output Print @rrange
	INSERT INTO com_menu([range], [padname], [padnum], [barname], [barnum], [prompname], [numitem], [hotkey], [progname], [E_], [n_], [r_], [t_], [i_], [o_], [b_], [x_], [Menutype], [Isactive], [puser], [mdefault], [labkey], [skipfor], [cprog], [mark], [xtvs_], [dsni_], [mcur_], [tds_], [newrange], [HRPay_], [eou_], [gst_]) 
	VALUES (@rrange, 'MAINMASTERS', 0, 'GSPAPILicenseMaster', 15, 'GSP API License Master', 0, '', 'DO UDCALLEXTPROG.APP WITH "udGSPAPILicUploader.EXE", "^'+CAST(@rrange AS VARCHAR)+'"', null, null, null, null, null, null, null, null, 'General', null, 'GSP API License Master', null, '', null, null, null, null, null, null, 0, null, 0, 0, 0)
END

/* Generated by SQLEditor */

IF NOT EXISTS (SELECT [PADNAME],[BARNAME] FROM COM_MENU WHERE padname='MAINMASTERREPORTS' AND barname='GSPAPILicenseMasterReport')
BEGIN
	Select @maxRange=max(Range)+1 From com_menu
	Execute [USP_Chk_ComMenuRange] @maxRange,@rrange= @rrange Output Print @rrange
	INSERT INTO com_menu([range], [padname], [padnum], [barname], [barnum], [prompname], [numitem], [hotkey], [progname], [E_], [n_], [r_], [t_], [i_], [o_], [b_], [x_], [Menutype], [Isactive], [puser], [mdefault], [labkey], [skipfor], [cprog], [mark], [xtvs_], [dsni_], [mcur_], [tds_], [newrange], [HRPay_], [eou_], [gst_]) 
	VALUES (@rrange, 'MAINMASTERREPORTS', 0, 'GSPAPILicenseMasterReport', 1, 'GSP API License Master Report', 0, '          ', 'DO UEREPORT WITH "GSP API License Master Report", "^'+CAST(@rrange AS VARCHAR)+'"', 1, 1, 1, 1, 1, 1, 1, 1, 'Report         ', 0, 'GSP API License Master Report', 0, '', '', '', 0, 1, 1, 1, 1, null, 1, 0, 0)
end