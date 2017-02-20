import os
import datetime
import shutil
import xml.dom.minidom
import codecs
from subprocess import call

def main():
    """ This is the main method. """
    long_version = get_new_version_long_form()
    short_version = get_new_version_short_form()
    old_version = get_old_version_from_SolutionVersionInfo()
    replace_solution_version(old_version, long_version)

    msbuild_path = r"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe"
    nsis_path = r"C:\Program Files (x86)\NSIS\makensis"
    sign_tool_path = r"C:\Program Files (x86)\Windows Kits\8.1\bin\x86\SignTool.exe"
    cert_path = r"X:\ComodoCodeSigningCert.pfx"
    password = input("Please enter code signing password:")
    version_info = "logview4net " + long_version
    exe_file = r".\src\App\bin\release\logview4net.exe"
    sign_log_file = "sign_log.txt"

    clean_solution()
    build_solution(msbuild_path)
    sign_exe(sign_tool_path, cert_path, password, version_info, exe_file, sign_log_file)
    make_installer(nsis_path)

    version_info = "logview4net " + long_version + " installer"
    exe_file = r".\src\setup\logview4net_setup.exe"
    sign_exe(sign_tool_path, cert_path, password, version_info, exe_file, sign_log_file)

    write_auto_update_version(short_version)
    update_pad()

    copy_files_to_site()

    input("Press Enter to continue...")


def sign_exe(sign_tool_path, cert_path, password, version_info, exe_file, sign_log_file):
    print("Signing " + exe_file)

    sign_command = r'"%s" sign /f "%s" /p "%s" /d "%s" /v /tr http://timestamp.comodoca.com "%s"' % (sign_tool_path, cert_path, password, version_info, exe_file) #,  sign_log_file)

    #sign_command = r'"%s" ' % (sign_tool_path)
    print(sign_command)
    call(sign_command)


def make_installer(nsis_path):
    """ Create the Nsis installer package. """
    print("Making installer")
    call(nsis_path + r" .\src\setup\logview4net.nsi")

def clean_solution():
    print("Clean solution")
    delete_file(r".\src\setup\logview4net.exe")
    delete_folder(r"src\app\bin\release")
    delete_folder(r"src\app\obj")
    delete_folder(r"src\setup\release")

def build_solution(msbuild_path):
    print("Build solution")
    call(msbuild_path + r" /t:Clean,Build /property:Configuration=Release /fileLogger src\logview4net.sln")

def write_auto_update_version(version):
    version_file = r".\src\Deployment\logview4net.version"
    write_auto_update_version_file(version, version_file)

    version_file = r".\site_hugo\static\dlfolder\logview4net.version"
    write_auto_update_version_file(version, version_file)

def write_auto_update_version_file(version, version_file):
    print("Creating version file " + version_file)

    delete_file(version_file)

    f2 = open(version_file, 'w')
    f2.write("logview4net auto update manifest\n")
    f2.write("Next manifest url\n")
    f2.write("http://logview4net.com/dlfolder/logview4net.version\n")
    f2.write("Available version\n")
    f2.write(version + "\n")
    f2.write("File url\n")
    f2.write("http://logview4net.com/dlfolder/logview4net_setup.exe")
    f2.close()


def replace_solution_version(old_version, new_version):
    solution_file = r".\src\Deployment\SolutionVersionInfo.cs"
    old_solution_file = solution_file + ".old"
    tmp_file = "tmp.cs"
    delete_file(old_solution_file)
    delete_file(tmp_file)

    f1 = open( solution_file, "r")
    f2 = open('tmp.cs', 'w')
    for line in f1:
        f2.write(line.replace(old_version, new_version))
    f1.close()
    f2.close()

    os.rename(solution_file, old_solution_file)
    os.rename(tmp_file, solution_file)


def get_new_version_long_form():
    year = datetime.date.today().isocalendar()[0] - 2000
    week = str(datetime.date.today().isocalendar()[1]).zfill(2)
    version = str(year) + "." + str(week) + ".0"
    return version

def update_pad():
    pad_file = r".\src\Deployment\logview4net.pad.xml"
    update_pad_file(pad_file)

    pad_file = r".\site_hugo\static\dlfolder\logview4net.pad.xml"
    update_pad_file(pad_file)

def update_pad_file(pad_file):
    print("Update PAD " + pad_file)
    pad_old = pad_file + ".old"
    delete_file(pad_old)
    os.rename(pad_file, pad_old)
    doc = xml.dom.minidom.parse(pad_old)

    replace_text(doc, "Program_Version", get_new_version_pad_form())
    replace_text(doc, "Program_Release_Month", datetime.datetime.now().strftime("%m"))
    replace_text(doc, "Program_Release_Day", datetime.datetime.now().strftime("%d"))
    replace_text(doc, "Program_Release_Year", datetime.datetime.now().strftime("%Y"))

    with codecs.open(pad_file, "w", "utf-8") as out:
        doc.writexml(out)

def replace_text(doc, node_name, newText):
    node = doc.getElementsByTagName(node_name)[0]

    if node.firstChild.nodeType != node.TEXT_NODE:
        raise Exception("node does not contain text")

    node.firstChild.replaceWholeText(newText)

def get_new_version_short_form():
    year = datetime.date.today().isocalendar()[0] - 2000
    week = str(datetime.date.today().isocalendar()[1]).zfill(2)
    version = str(year) + str(week) + "0"
    return version

def get_new_version_pad_form():
    year = datetime.date.today().isocalendar()[0] - 2000
    week = str(datetime.date.today().isocalendar()[1]).zfill(2)
    version = str(year) + "." + str(week)
    return version


def get_old_version_from_SolutionVersionInfo():
    ins = open( r".\src\Deployment\SolutionVersionInfo.cs", "r" )

    for line in ins:
        if "AssemblyVersion" in line:
            version = extract(line, "\"", "\"")
    ins.close()
    return version
	
def extract(raw_string, start_marker, end_marker):
    start = raw_string.index(start_marker) + len(start_marker)
    end = raw_string.index(end_marker, start)
    return raw_string[start:end]


def delete_file(file):
    try:
        os.remove(file)
    except OSError:
        pass

def copy_files_to_site():
    shutil.copyfile(r".\src\setup\logview4net_setup.exe", r".\site_hugo\static\dlfolder\logview4net_setup.exe")
    shutil.copyfile(r".\src\Deployment\logview4net.version", r".\site_hugo\static\dlfolder\logview4net.version")
    shutil.copyfile(r".\src\Deployment\logview4net.pad.xml", r".\site_hugo\static\dlfolder\logview4net.pad.xml")


def delete_folder(folder):
    try:
        shutil.rmtree(folder)
    except OSError:
        pass

###########################################################################################
main()
