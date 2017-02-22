
; example2.nsi
;
; This script is based on example1.nsi, but it remember the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install example2.nsi into a directory that the user selects,
;--------------------------------
!include LogicLib.nsh
!include "DotNET.nsh"



; The name of the installer
Name "logview4net"

; The file to write
OutFile "logview4net_setup.exe"

; The default installation directory
InstallDir $PROGRAMFILES\logview4net

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\logview4net" "Install_Dir"

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "logview4net (required)"

	 
  SectionIn RO
    
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "..\App\bin\release\logview4net.exe"
  File "..\App\bin\release\logview4net.core.dll"
  File "..\App\bin\release\logview4net.mslisteners.dll"
  File "..\App\bin\release\logview4net.mysqllistener.dll"
  File "..\App\DefaultSession.xml"
  File "..\..\README.md"
  File "..\Deployment\LICENSE.RTF"
  File "..\App\bin\release\MySql.data.dll"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\logview4net "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\logview4net" "DisplayName" "logview4net"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\logview4net" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\logview4net" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\logview4net" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\logview4net"
  CreateShortCut "$SMPROGRAMS\logview4net\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\logview4net\logview4net.lnk" "$INSTDIR\logview4net.exe" "" "$INSTDIR\logview4net.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\logview4net"
  DeleteRegKey HKLM SOFTWARE\logview4net

  ; Remove files and uninstaller
  Delete $INSTDIR\logview4net.exe
  Delete $INSTDIR\logview4net.core.dll"
  Delete $INSTDIR\logview4net.mslisteners.dll"  
  Delete $INSTDIR\logview4net.mysqllistener.dll"  
  Delete $INSTDIR\README.md
  Delete $INSTDIR\LICENSE.RTF
  Delete $INSTDIR\DefaultSession.xml
  Delete $INSTDIR\uninstall.exe
  
  
  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\logview4net\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\logview4net"
  RMDir "$INSTDIR"

SectionEnd
