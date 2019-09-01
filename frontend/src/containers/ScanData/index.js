import React, { Component } from "react";
import { connect } from 'react-redux';
import { CircularProgress } from 'components/uielements/progress';
import Form from './Form';
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
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import { scanDataActions } from 'redux/actions';
import { Date, Time } from 'helpers/moment';
import _ from 'lodash';
import "styles/style.css";

class ScanData extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            toggle: false,
            delete: false,
            params: {}
        }
    }
    componentDidMount() {
        this.props.getScanData(this.onSuccess, this.onSuccess)
    }
    onSuccess = () => {
        this.setState({
            loading: false
        })
    }
    renderData = () => {
        return _.map(this.props.datas, item => (
            <TableRow>
                <TableCell padding="checkbox"><Checkbox /></TableCell>
                <TableCell>{Date(item.createdOn)}</TableCell>
                <TableCell>{Time(item.createdOn)}</TableCell>
                <TableCell>{item.dataType}</TableCell>
                <TableCell>{item.status}</TableCell>
            </TableRow>
        ))
    }

    onToggleForm = (status, params = {}) => {
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

    render() {
        if (this.state.loading) {
            return <CircularProgress />;
        }
        return (
            <LayoutWrapper>
                <FullColumn>
                    <Papersheet title="Scan Data">
                        <Grid container justify="flex-end">
                            <Grid item xs={3}>
                                <FormControl variant="outlined" margin="dense" fullWidth>
                                    <Select native input={<OutlinedInput />}>
                                        <option value="">None</option>
                                        <option value="PC_M3">PC_M3</option>
                                    </Select>
                                </FormControl>
                            </Grid>
                        </Grid>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell padding="checkbox"><Checkbox /></TableCell>
                                    <TableCell>Date</TableCell>
                                    <TableCell>Time</TableCell>
                                    <TableCell>Data</TableCell>
                                    <TableCell>Status</TableCell>
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
                                Add new scan data
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                            >
                                Copy selected
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
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
                    <Form onToggle={this.onToggleForm} status={this.state.toggle} params={this.state.params}/>
                    <DeleteAlert status={this.state.delete} onSubmit={this.delete} onClose={this.onToggleDelete}/>
                </FullColumn>
            </LayoutWrapper>
        );
    }
}
const mapSateToProps = state => {

    return {
        datas: state.ScanData.list
    };
};

const mapDispatchToProps = {
    getScanData: scanDataActions.getScanData
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(ScanData);