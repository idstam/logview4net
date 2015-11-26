import os
from subprocess import call

def extract(raw_string, start_marker, end_marker):
    start = raw_string.index(start_marker) + len(start_marker)
    end = raw_string.index(end_marker, start)
    return raw_string[start:end]
	
ins = open( r".\src\Deployment\SolutionVersionInfo.cs", "r" )

for line in ins:
	if "AssemblyVersion" in line:
		version = extract(line, "\"", "\"")

ins.close()


print("Found version number " + version)

try:
    os.remove("tmp.cmd")
except OSError:
    pass
	
f1 = open('make_setup_release_zip.cmd', 'r')
f2 = open('tmp.cmd', 'w')
for line in f1:
    f2.write(line.replace('<[version]>', version))
f1.close()
f2.close()

call(["tmp.cmd"])

