import React, { Component } from "react";
import { connect } from 'react-redux';
import Form from './Form';
import { CircularProgress } from 'components/uielements/progress';
import DeleteAlert from './Alert';
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import { FullColumn } from "components/utility/rowColumn";
import Button from "components/uielements/button/index.js";
import Grid from "components/uielements/grid";
import Table from "components/uielements/table";
import Checkbox from 'components/uielements/checkbox';
import {
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from 'components/uielements/table';
import { companyActions } from 'redux/actions';
import _ from 'lodash';
import "styles/style.css";

class Company extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            toggle: false,
            delete: false,
            params: {},
            editAble: false,
            selected: []
        }
    }
    
    componentDidMount() {
        this.props.get(this.onSuccess, this.onSuccess)
    }
    
    onSuccess = () => {
        this.setState({
            loading: false
        })
    }
    
    
    
    onToggleForm = (status, params = {}) => {
        this.setState({
            toggle: status,
            params: params
        })
    }

    onToggleFormEdit = (status, params = {}) => {
        this.setState({
            toggle: status,
            params: params
        })
    }
    
    onToggleDelete = (status) => {
        this.setState({
            delete: status
        })
    }
    
    delete = () => {

    }

    onSubmit = (params) => {
        if(this.state.editAble) {
            this.props.edit(params);
        } else {
            this.props.add(params);
        }
    }

    renderData = () => {
        return _.map(this.props.companies, (item, key) => (
            <TableRow key={key}>
                <TableCell padding="checkbox">
                    <Checkbox 
                        onChange={() => this.onSelected(item.cid)} 
                        checked={_.includes(this.state.selected, item.cid)} 
                    />
                </TableCell>
                <TableCell>{item.name}</TableCell>
                <TableCell>{item.address}</TableCell>
                <TableCell>{item.remark}</TableCell>
            </TableRow>
        ))
    }

    onSelected = id => {
        const { selected } = this.state;
        
        if ( _.includes(selected, id)) {
            this.setState({
                selected: _.filter(selected, item => {
                    return item !== id
                })
            })
        } else {
            selected.push(id);
            this.setState({
                selected: selected
            })
        }
    }

    onSelectedAll = () => {
        const { selected } = this.state;
        const { companies } = this.props;
        
        if ( selected.length === companies.length ){
            this.setState({
                selected: []
            })
        } else {
            this.setState({
                selected: _.map(companies, item => {
                    return item.cid
                })
            })
        }
    }
    
    render() {
        if (this.state.loading) {
            return <CircularProgress />;
        }
        return (
            <LayoutWrapper>
                <FullColumn>
                    <Papersheet title="Companies">
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell padding="checkbox"><Checkbox onChange={() => this.onSelectedAll()} /></TableCell>
                                    <TableCell>Name</TableCell>
                                    <TableCell>Address</TableCell>
                                    <TableCell>Remark</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {this.renderData()}
                            </TableBody>
                        </Table>
                        <Grid
                            container
                            direction="row"
                            justify="center"
                            alignItems="center"
                        >
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                                onClick={() => this.onToggleForm(true)}
                            >
                                Add new company
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                                onClick={() => this.onToggleFormEdit(true)}
                            >
                                Edit selected
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                                onClick={() => this.onToggleDelete(true)}
                            >
                                Delete selected
                            </Button>
                        </Grid>
                    </Papersheet>
                    <Form 
                        onToggle={this.onToggleForm} 
                        status={this.state.toggle} 
                        params={this.state.params}
                        onSubmit={this.onSubmit}
                    />
                    <DeleteAlert status={this.state.delete} onSubmit={this.delete} onClose={this.onToggleDelete}/>
                </FullColumn>
            </LayoutWrapper>
        );
    }
}
const mapSateToProps = state => {
    return {
        companies: state.Company.list
    };
};

const mapDispatchToProps = {
    get: companyActions.get,
    add: companyActions.add,
    edit: companyActions.edit,
    delete: companyActions.delete
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(Company);