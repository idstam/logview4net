import os
import datetime
import shutil
import xml.dom.minidom
import codecs
from subprocess import call
import subprocess

def main():
    """ This is the main method. """
    long_version = get_new_version_long_form()
    short_version = get_new_version_short_form()
    old_version = get_old_version_from_SolutionVersionInfo()
    replace_solution_version(old_version, long_version)

    ##msbuild_path = r"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
    nsis_path = r"C:\Program Files (x86)\NSIS\makensis.exe"
    exe_file = r".\src\App\bin\release\logview4net.exe"
    setup_file = r".\src\setup\logview4net_setup.exe"

    ##clean_solution()
    ##build_solution(msbuild_path)
    make_installer(nsis_path)

    update_file_hash(exe_file, setup_file)

    write_auto_update_version(short_version)
    update_pad()

    copy_files_to_site()

    ###create_site()

    input("Press Enter to continue...")

def update_file_hash(exe_file, setup_file):
    exe_hash = get_file_hash(exe_file).decode("utf-8") 
    print('Exe Hash:' + exe_hash)

    setup_hash = get_file_hash(setup_file)
    print('Setup Hash:' + exe_hash)

    replace_hash_md(exe_hash, setup_hash)
    replace_hash_html(exe_hash, setup_hash)

def get_file_hash(source):
    """Calculate the Sha256 hash of a file. """

    output = subprocess.Popen(['certutil', "-hashfile", source, "sha256"], stdout=subprocess.PIPE).communicate()[0]
    file_hash = output.split(b'\r\n')[1]

    return file_hash

def make_installer(nsis_path):
    """ Create the Nsis installer package. """
    print("Making installer")
    call(nsis_path + r" .\src\setup\logview4net.nsi")

def create_site():
    """ Create the site using Hugo. """
    print("Create the site using Hugo.")
    call(r"./site_hugo/hugo")

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

def replace_hash_md(exe_hash, setup_hash):
    print('Relace file hashes in README.md')
    readme_file = r".\README.md"
    old_readme_file = readme_file + ".old"
    tmp_file = "tmp.cs"
    delete_file(old_readme_file)
    delete_file(tmp_file)

    readme_handle = open(readme_file, "r")
    temp_handle = open('tmp.cs', 'w')
    for line in readme_handle:
        if 'Sha256 hash of installer:' in line:
            temp_handle.write('  * Sha256 hash of installer: ' + setup_hash.decode('utf-8') + '\n')
        elif 'Sha256 hash of logview4net.exe:' in line:
            temp_handle.write('  * Sha256 hash of logview4net.exe: ' + exe_hash + '\n')
        else:
            temp_handle.write(line)

    readme_handle.close()
    temp_handle.close()

    os.rename(readme_file, old_readme_file)
    os.rename(tmp_file, readme_file)

def replace_hash_html(exe_hash, setup_hash):
    print('Relace file hashes in index.html')
    readme_file = r".\site_hugo\layouts\index.html"
    old_readme_file = readme_file + ".old"
    tmp_file = "tmp.cs"
    delete_file(old_readme_file)
    delete_file(tmp_file)

    readme_handle = open(readme_file, "r")
    temp_handle = open('tmp.cs', 'w')
    for line in readme_handle:
        if 'Sha256 hash of installer:' in line:
            temp_handle.write('<p>Sha256 hash of installer: ' + setup_hash.decode('utf-8') + '</p>\n')
        elif 'Sha256 hash of logview4net.exe:' in line:
            temp_handle.write('<p>Sha256 hash of logview4net.exe: ' + exe_hash + '</p>\n')
        else:
            temp_handle.write(line)

    readme_handle.close()
    temp_handle.close()

    os.rename(readme_file, old_readme_file)
    os.rename(tmp_file, readme_file)

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


def delete_folder(folder):
    try:
        shutil.rmtree(folder)
    except OSError:
        pass

###########################################################################################
main()
