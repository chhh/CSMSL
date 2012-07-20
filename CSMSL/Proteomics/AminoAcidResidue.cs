﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSMSL.Chemistry;

namespace CSMSL.Proteomics
{
    public class AminoAcidResidue: IEquatable<AminoAcidResidue>, IChemicalFormula, IMass
    {        
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private char _letter;

        public char Letter
        {
            get { return _letter; }
            set { _letter = value; }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private ChemicalFormula _chemicalFormula;

        public ChemicalFormula ChemicalFormula
        {
            get { return _chemicalFormula; }
            private set { _chemicalFormula = value; }
        }

        public Mass Mass
        {
            get { return _chemicalFormula.Mass; }
        }
        
        public AminoAcidResidue(string name, char oneLetterAbbreviation, string threeLetterAbbreviation, ChemicalFormula chemicalFormula)
        {
            _name = name;
            _letter = oneLetterAbbreviation;
            _symbol = threeLetterAbbreviation;
            _chemicalFormula = chemicalFormula;            
        }

        public override string ToString()
        {
            return _name;
        }

        public bool Equals(AminoAcidResidue other)
        {
            return _chemicalFormula.Equals(other._chemicalFormula);
        }
    }
}