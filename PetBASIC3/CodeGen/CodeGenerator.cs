using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using PetBASIC3.AST;

namespace PetBASIC3.CodeGen
{
    public class CodeGenerator
    {
        #region LIBString

        private const string lib = @"
	ORG 32768
	ld a, 2
	call 5633
	jp main
print_str:
	ld a, (hl)
	or a
	ret z
.loop:
	push hl
	call $0010
	pop hl
	inc hl
	ld a, (hl)
	or a
	jp nz, .loop
	ret
print_num:
	call $2D2B
	jp $2DE3
mult_hl_de:
	ld b, h
    ld c, l
	ld hl, 0
	sla  e
	rl d
	jr nc, $+4
	ld h, b
	ld l, c
	ld a, 15
_mult_loop:
	add hl, hl
	rl e
	rl d
	jr nc, $+6
	add hl, bc
	jr nc, $+3
	inc de
	dec a
	jr nz, _mult_loop
	ret
div_bc_de:
	ld a, b
	ld hl, 0
	ld b, 16
_div_loop:
	sll c
	rla
	adc hl, hl
	sbc hl, de
	jr nc, $+4
	add hl, de
	dec c
	djnz _div_loop
	ret
plot_xy:
	push af
	push bc
	push hl
	ld a,b
	call calc_y_addr
	ld a,c
	and %11111000
	srl a
	srl a
	srl a
	or l
	ld l,a
	ld a,c
	and %00000111
	ld b,%10000000
_pixel_shift:
	cp 0
	jr z,_shift_done 
	srl b
	dec a
	jr _pixel_shift
_shift_done:
	ld a,b
	or (hl)
	ld (hl),a 
	pop hl
	pop bc
	pop af
	ret 
calc_y_addr:
	ld hl,$4000
	push af
	and %00000111
	or h
	ld h,a
	pop af
	push af
	and %00111000
	sla a
	sla a
	or l
	ld l,a
	pop af
	push af
	and %11000000
	srl a
	srl a
	srl a
	or h
	ld h,a
	pop af
	ret
input:
	ld de, 0
.wait_key:
	halt
	LD A,(23611)
	AND %00100000
	JP Z,.wait_key
	LD A,(23611)
	AND %11011111
	LD (23611), A
	LD A,(23560)
	LD (buffer),A
	ld hl, buffer
	push af
	push de
	call print_str
	pop de
	pop af
	CP 13
	JP Z,.enter
	sub 48
	ld hl, 0
	add hl, de
	add hl, hl
	ld b, h
	ld c, l
	add hl, hl
	add hl, hl
	add hl, bc
	ld b, 0
	ld c, a
	add hl, bc
	ld d, h
	ld e, l
	jp .wait_key
.enter:
	ld b, d
	ld c, e
	ret
buffer: DB 0,0
main:
";
        #endregion

        private StringBuilder sb;
        private List<OP> _ops = new List<OP>(); 

        public CodeGenerator()
        {
            sb = new StringBuilder(lib);
        }

        public void End()
        {
            foreach (var op in _ops)
            {
                sb.AppendFormat("{0}\n", op);
            }
            sb.AppendLine("\tjp $2D2B");
            sb.AppendLine("vars: dw 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
            for (var i = 0; i < StringNode.constants.Count; i++)
            {
                var constant = StringNode.constants[i];
                sb.AppendFormat("str{0}:\n\tdb {1},0\n", i, constant);
            }
        }

        public void Emit(string op)
        {
            _ops.Add(new OP(op));
        }

        private const bool OptimizePushPop = false;

        public void Emit(string op, string arg1)
        {
            if (OptimizePushPop && op == "pop" && _ops.Last().Op == "push")
            {
                var push = _ops.Last();
                _ops.RemoveAt(_ops.Count - 1);
                if (arg1 == push.Arg1)
                    return;
                _ops.Add(new OP("ld", "" + arg1[0], "" + push.Arg1[0]));
                _ops.Add(new OP("ld", "" + arg1[1], "" + push.Arg1[1]));
                return;
            }
            _ops.Add(new OP(op, arg1));
        }

        public void Emit(string op, string arg1, string arg2)
        {
            _ops.Add(new OP(op, arg1, arg2));
        }

        public void Label(string lbl)
        {
            _ops.Add(new OP("label", lbl));
        }

        public override string ToString() => sb.ToString();

        private class OP
        {
            public readonly string Op;
            public readonly string Arg1;
            public readonly string Arg2;

            public OP(string op, string arg1 = null, string arg2 = null)
            {
                Op = op;
                Arg1 = arg1;
                Arg2 = arg2;
            }

            public override string ToString()
            {
                if (Arg2 != null)
                    return string.Format("\t{0} {1}, {2}", Op, Arg1, Arg2);
                if (Arg1 != null)
                    if (Op == "label")
                        return Arg1 + ":";
                    else
                        return string.Format("\t{0} {1}", Op, Arg1);
                return "\t" + Op;
            }
        }
    }
}
