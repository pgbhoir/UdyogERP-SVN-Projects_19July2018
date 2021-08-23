                                   
Declare @rrange Numeric,@maxRange Numeric

If Not Exists (Select padname,barname From Com_Menu Where PadName='SETTINGS' and BarName='ACCOUNTSSETTINGS')
Begin
	Select @rrange=max([Range])+1 From COM_MENU where padname='SETTINGS'
	Execute [USP_Chk_ComMenuRange] @maxRange,@rrange = @rrange output print @rrange
	INSERT INTO COM_MENU([range], [padname], [padnum], [barname], [barnum], [prompname], [numitem], [hotkey], [progname], [E_], [n_], [r_], [t_], [i_], [o_], [b_], [x_], [Menutype], [Isactive], [puser], [mdefault], [labkey], [skipfor], [cprog], [mark], [xtvs_], [dsni_], [mcur_], [tds_], [newrange], [HRPay_], [eou_], [gst_], [showbutton], [buttoncap], [buttonord]) 
	VALUES (@rrange, 'SETTINGS                                          ', 0, 'ACCOUNTSSETTINGS                                  ', 9, 'Accounts Settings                                           ', 1, '          ', '', null, null, null, null, null, null, null, null, 'General        ', null, 'Accounts Settings                                                                                   ', null, null, null, null, null, null, null, null, 0, null, 0, 0, 0, 0, '                         ', 0)

END

If Not Exists (Select padname,barname From Com_Menu Where PadName='ACCOUNTSSETTINGS' and BarName='CASHFLOWSETTINGS')
Begin
	Select @rrange=max([Range])+1 From COM_MENU where padname='ACCOUNTSSETTINGS'
	Execute [USP_Chk_ComMenuRange] @maxRange,@rrange = @rrange output print @rrange
	INSERT INTO COM_MENU([range], [padname], [padnum], [barname], [barnum], [prompname], [numitem], [hotkey], [progname], [E_], [n_], [r_], [t_], [i_], [o_], [b_], [x_], [Menutype], [Isactive], [puser], [mdefault], [labkey], [skipfor], [cprog], [mark], [xtvs_], [dsni_], [mcur_], [tds_], [newrange], [HRPay_], [eou_], [gst_], [showbutton], [buttoncap], [buttonord]) 
	VALUES (@rrange, 'ACCOUNTSSETTINGS                                  ', 0, 'CASHFLOWSETTINGS                                  ', 1, 'Cash Flow Settings                                          ', 0, '          ', 'DO UECASHFLOW WITH "^'+ltrim(rtrim(cast(@rrange as varchar)))+'"', null, null, null, null, null, null, null, null, 'General        ', null, 'Cash Flow Settings                                                                                  ', null, null, null, null, null, null, null, null, 0, null, 0, 0, 0, 0, '                         ', 0)

END


If Not Exists(Select Rep_nm From r_status where [Group]='CASH FLOW' and [desc]='User Defined Cash Flow' and Rep_nm='CASHFLOW')
Begin
INSERT INTO r_status([group], [desc], [rep_nm], [rep_nm1], [defa], [ismethod], [isfr_date], [isto_date], [isfr_ac], [isto_ac], [isfr_item], [isto_item], [isfr_amt], [isto_amt], [isdept], [iscategory], [iswarehous], [isinvseri], [vou_type], [spl_condn], [qTable], [runbefore], [runafter], [isfr_area], [isto_area], [dontshow], [methodof], [repfcondn], [repfcodn], [e_], [x_], [i_], [o_], [n_], [r_], [t_], [b_], [ac_group], [it_group], [RetTable], [SQLQUERY], [ZOOMTYPE], [spWhat], [xtvs_], [dsni_], [mcur_], [retfilenm], [tds_], [DISPORD], [isrule], [newgroup], [HRPay_], [ExpFileNm], [EMailSub], [EmailBody], [PGBRKFLD], [PgBrakFld], [copyname], [Datecaption], [zoominfld], [AUTOEMAIL], [isrpttbl], [isfixtbl], [RepoCap], [eou_], [gst_], [RepFor], [zoomadflds], [iscostcent], [it_type]) 
VALUES ('CASH FLOW                                                   ', 'User Defined Cash Flow                                         ', 'CASHFLOW            ', '                    ', 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '                                                                                                                                                                                                                                                          ', '', '               ', '                                                                                                    ', '                    ', 0, 0, 0, '                                                                                                                                                                                                                                                          ', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', '_CASHFLOW           ', 'EXECUTE usp_REP_CASHFLOWSTATEMENT;', ' ', 'P', 0, 0, 0, '                                                  ', 0, 0, 0, null, 0, '                              ', '', '', '                                                            ', '                                                            ', '', '                              ', '          ', 0, 0, 0, 'User Defined Cash Flow                                                                              ', 0, 0, 0, '                                                                                                                                                                                                                                                          ', 0, '')

End

